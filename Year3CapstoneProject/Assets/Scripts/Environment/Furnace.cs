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
	private float delay;			// Delay between fire spews
	#endregion
	public bool IsOn {  get { return isOn; } }

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(FireOn());
	}


	private IEnumerator FireOn()
	{
		// If the blocks are inactive, wait until the block becomes active again.
		while (!GetComponent<Platform>().effectsActive || GetComponent<Platform>().IsDropped)
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
