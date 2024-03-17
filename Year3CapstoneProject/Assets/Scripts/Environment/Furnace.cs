using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour
{
	[SerializeField]
	private bool isOn;

	[SerializeField]
	private ParticleSystem[] flameArray;

	[SerializeField]
	private int damage;

	[SerializeField]
	private float burnTime;

	[SerializeField]
	private BoxCollider trigger;

	[SerializeField]
	AudioSource flameSoundSource;

	//not to be confused with damage, this is whether or not a player has been damaged
	private bool damaged;

	//the time a block waits to re-ignite its fire
	private float delay;

	private Coroutine coroutine;

	// Start is called before the first frame update
	void Start()
	{
		coroutine = StartCoroutine(FireOn());
	}

	// In update, handle turning the flame effect on and off visually.
	private void Update()
	{
		if (!GetComponent<Platform>().effectsActive)
		{
			StopAllCoroutines();
			coroutine = null;
		}
		else
		{
			if (coroutine == null)
				coroutine = StartCoroutine(FireOn());
		}

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

	//if damaged is true, wait a second and then turn it false
	private IEnumerator AllowDamage()
	{
		if (damaged)
		{
			yield return new WaitForSeconds(1f);
			damaged = false;
		}
	}

	//alternates the furnace between being on for (burnTime), then off for (delay)
	private IEnumerator FireOn()
	{
		delay = Random.Range(2.0f, 5.0f);
		yield return new WaitForSeconds(delay);
		//turn on fire
		for (int i = 0; i < flameArray.Length; i++)
		{
			flameArray[i].Emit(1);
		}

		yield return new WaitForSeconds(2);

		for (int i = 0; i < flameArray.Length; i++)
		{
			flameArray[i].Play();
			FlameSound();
		}
		damaged = false;
		isOn = true;
		trigger.enabled = true;
		yield return new WaitForSeconds(burnTime);

		coroutine = StartCoroutine(FireOff());
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
		coroutine = StartCoroutine(FireOn());
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
