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

	private Coroutine coroutine;

	private bool hasRespawned = true;

	[SerializeField]
	MeshRenderer theMesh;

	//If there is a player standing on this, call a timer coroutine that crumbles the block
	void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player") && hasRespawned)
		{
			if (coroutine == null)
			{
				Debug.Log("We are here~!");
				coroutine = StartCoroutine(CrumbleTimer());
			}
		}

	}
	//If there is a player standing on this, call a timer coroutine that crumbles the block
	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (coroutine != null)
			{
				StopCoroutine(coroutine);
				coroutine = null;
			}
		}

	}
	//Timer coroutine that waits breakTime, crumbles the block, then re-instantiates it.
	private IEnumerator CrumbleTimer()
	{
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

		hasRespawned = true;
		coroutine = null;
	}
}
