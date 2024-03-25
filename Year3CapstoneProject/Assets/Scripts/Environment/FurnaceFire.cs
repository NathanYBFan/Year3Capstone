using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceFire : MonoBehaviour
{
	[SerializeField]
	private Furnace furnace;

	[SerializeField]
	[Header("Furnace Stats")]
	[Foldout("Dependencies"), Tooltip("How much damage the furnaces deal.")]
	private int damage;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("The interval that damage gets applied by.\nEx. A value of 1 means damage will be applied per 1 second.")]
	private float damageInterval = 1;


	private bool damaged;           // Has a player taken damage?

	private void OnEnable()
	{
		damaged = false;
	}
	private void OnDisable()
	{
		damaged = false;
	}
	void OnTriggerStay(Collider other)
	{
		if (!transform.parent.gameObject.GetComponent<Platform>().effectsActive) return;
		//if the colliding object is a bullet,  delete it if the fire is on
		if (other.gameObject.GetComponentInChildren<CapsuleCollider>() != null && other.gameObject.GetComponentInChildren<BulletBehaviour>())
		{
			if (furnace.IsOn)
				BulletObjectPoolManager._Instance.ExpiredBullet(other.gameObject);


		}
		//if the colliding object is a player (check tag), try to deal damage
		if (other.gameObject.GetComponentInChildren<CapsuleCollider>() != null && other.gameObject.GetComponentInChildren<CapsuleCollider>().CompareTag("Player"))
		{
			//damage the player that made contact, only if the fire is on
			if (furnace.IsOn && !damaged)
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
}
