using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour
{
	#region Serialize Fields
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("The flame particle emitters.")]
	private ParticleSystem[] flameArray;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	AudioSource flameSoundSource;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("The damage hitbox")]
	private BoxCollider trigger;

	[SerializeField]
	[Header("Furnace Stats")]
	[Foldout("Dependencies"), Tooltip("How much damage the furnaces deal.")]
	private int damage;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("The interval that damage gets applied by.\nEx. A value of 1 means damage will be applied per 1 second.")]
	private float damageInterval = 1;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("How long the fire lasts for.")]
	private float burnTime;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("The min time a furnace could be inactive for.")]
	private float minWaitTime = 4;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("The max time a furnace could be inactive for.")]
	private float maxWaitTime = 7;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("The time between the initial warning shot and the actual fire spew.")]
	private float timeBeforeActive = 2;
	#endregion
	#region Private Variables
	private bool isOn;				// True if furnace is currently burning
	private bool damaged;			// Has a player taken damage?
	private float delay;			// Delay between fire spews
	#endregion

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(FireOn());
	}

	void OnTriggerStay(Collider other)
	{
		if (!GetComponent<Platform>().effectsActive) return;
		//if the colliding object is a bullet,  delete it if the fire is on
		if (other.gameObject.GetComponentInChildren<CapsuleCollider>() != null && other.gameObject.GetComponentInChildren<BulletBehaviour>())
		{
			if (isOn)
				BulletObjectPoolManager._Instance.ExpiredBullet(other.gameObject);


		}
		//if the colliding object is a player (check tag), try to deal damage
		if (other.gameObject.GetComponentInChildren<CapsuleCollider>() != null && other.gameObject.GetComponentInChildren<CapsuleCollider>().CompareTag("Player"))
		{
			//damage the player that made contact, only if the fire is on
			if (isOn && !damaged)
			{
				other.transform.parent.parent.GetComponent<PlayerStats>().TakeDamage(damage, DamageType.Hazard);
				damaged = true;
				StartCoroutine(AllowDamage());
			}
		}

	}

	private IEnumerator AllowDamage()
	{
		if (damaged)
		{
			// If damaged is true, wait a second and then turn it false
			yield return new WaitForSeconds(damageInterval);
			damaged = false;
		}
	}

	private IEnumerator FireOn()
	{
		// If the blocks are inactive, wait until the block becomes active again.
		while (!GetComponent<Platform>().effectsActive)
			yield return null;

		// Waiting until we should give a warning shot.
		delay = Random.Range(minWaitTime, maxWaitTime);
		yield return new WaitForSeconds(delay);

		// Warning given!
		for (int i = 0; i < flameArray.Length; i++)
		{
			flameArray[i].Emit(1);
		}

		// Waiting until we go full blast with fire.
		yield return new WaitForSeconds(timeBeforeActive);

		// BURN!
		for (int i = 0; i < flameArray.Length; i++)
		{
			flameArray[i].Play();
			FlameSound();
		}
		damaged = false;
		isOn = true;
		trigger.enabled = true;
		yield return new WaitForSeconds(burnTime);

		// Turn off the furnace.
		StartCoroutine(FireOff());
		yield break;

	}
	private IEnumerator FireOff()
	{
		for (int i = 0; i < flameArray.Length; i++)
		{
			flameArray[i].Stop(true, ParticleSystemStopBehavior.StopEmitting);

		}
		trigger.enabled = false;
		damaged = false;
		isOn = false;
		StartCoroutine(FireOn());
		yield break;
	}

	//Plays the player damage sound
	private void FlameSound()
	{
		float randPitch = Random.Range(0.8f, 1.5f);
		AudioSource audioSource = AudioManager._Instance.ChooseEnvAudioSource();
		if (audioSource != null)
		{
			audioSource.pitch = randPitch;
			AudioManager._Instance.PlaySoundFX(AudioManager._Instance.EnvAudioList[1], audioSource);
		}

	}
}
