using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumbleBlock : MonoBehaviour
{
	[SerializeField]
	private float breakTime;

	[SerializeField]
	private float respawnTime;

	[SerializeField]
	Collider boxCollider;

	[SerializeField]
	Collider crumbleTrigger;

	public GameObject destroyedVersion;

	public GameObject instantiatedExplosion;

	[SerializeField]
	Platform thePlatform;

	private Coroutine crumbleCoroutine;
	private Coroutine shakeCoroutine;

	private bool hasRespawned = true;

	[SerializeField]
	MeshRenderer theMesh;

	float shakeDelay = 0.025f;
	float shakeAmount = 0.25f;
	float shakeDuration = 0.15f;
	Vector3 startingPos;
	public bool HasRespawned { get { return hasRespawned; } }

	private void Awake()
	{
		startingPos = transform.position;
	}
	//If there is a player standing on this, call a timer crumbleCoroutine that crumbles the block
	void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player") && hasRespawned && transform.parent.GetComponent<Platform>().effectsActive == true)
		{
			if (crumbleCoroutine == null) crumbleCoroutine = StartCoroutine(CrumbleTimer());
		}

	}
	//If there is a player standing on this, call a timer crumbleCoroutine that crumbles the block
	void OnTriggerExit(Collider other)
	{
		transform.localPosition = Vector3.zero;
		if (other.CompareTag("Player") && transform.parent.GetComponent<Platform>().effectsActive == true)
		{
			if (crumbleCoroutine != null)
			{
				StopCoroutine(crumbleCoroutine);
				crumbleCoroutine = null;
			}
		}

	}
	private IEnumerator Shake()
	{
		float currTime = 0;
		Vector3 randomPos;
		while (currTime <= shakeDuration)
		{
			currTime += Time.deltaTime;
			Vector2 xzRandomPos = Random.insideUnitCircle;
			randomPos = startingPos + (new Vector3(xzRandomPos.x, 0, xzRandomPos.y) * shakeAmount);
			transform.position = randomPos;
			yield return new WaitForSeconds(shakeDelay);
		}
		transform.localPosition = Vector3.zero;
	}
	//Timer crumbleCoroutine that waits breakTime, crumbles the block, then re-instantiates it.
	private IEnumerator CrumbleTimer()
	{
		CrackSound();
		StartCoroutine(Shake());
		float currTime = breakTime;
		while (currTime >= 0)
		{
			currTime -= Time.deltaTime;
			yield return null;
		}

		//Crumble the block
		instantiatedExplosion = Instantiate(destroyedVersion, transform.position, transform.rotation);

		thePlatform.collapse();
		boxCollider.enabled = false;
		theMesh.enabled = false;
		crumbleTrigger.enabled = false;
		hasRespawned = false;

		StartCoroutine(RespawnBlock());
	}

	private IEnumerator RespawnBlock()
	{
		transform.localPosition = Vector3.zero;
		float currTime = respawnTime;
		while (currTime >= 0)
		{
			currTime -= Time.deltaTime;
			yield return null;
		}
		Destroy(instantiatedExplosion.gameObject);

		boxCollider.enabled = true;
		theMesh.enabled = true;
		crumbleTrigger.enabled = true;
		thePlatform.rise();
		instantiatedExplosion = null;
		hasRespawned = true;
		crumbleCoroutine = null;
	}

	//Plays the tile cracking sound
	private void CrackSound()
	{
		float randPitch = UnityEngine.Random.Range(0.75f, 0.95f);
		AudioSource audioSource = AudioManager._Instance.ChooseEnvAudioSource();
		if (audioSource != null)
		{
			audioSource.pitch = randPitch;
			AudioManager._Instance.PlaySoundFX(AudioManager._Instance.EnvAudioList[2], audioSource);
		}

	}
}
