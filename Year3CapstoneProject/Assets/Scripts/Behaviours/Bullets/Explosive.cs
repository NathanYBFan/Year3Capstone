using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
	[SerializeField]
	[Foldout("Stats"), Tooltip("The damage the explosion deals to damageable entities.")]
	private int damage;
	[SerializeField, ReadOnly]
	[Foldout("Stats"), Tooltip("If this explosion can give a debuff, this is where it would be")]
	private Debuff giveableDebuff;
	[SerializeField]
	[Foldout("Stats"), Tooltip("Float value that affects the scaling of the explosion radius")]
	private float minScale, maxScale;
	[SerializeField]
	[Foldout("Stats"), Tooltip("Speed of which the explosion radius expands.")]
	private float explosionSpeed;

	[SerializeField, ReadOnly]
	[Foldout("Stats"), Tooltip("")]
	public int originalPlayerIndex;

	[SerializeField, ReadOnly]
	[Foldout("Stats"), Tooltip("")]
	public PlayerStats playerOwner;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			//Burn effect.
			if (other.transform.parent.parent.GetComponent<PlayerBody>().PlayerIndex != originalPlayerIndex)
			{
				if (playerOwner.giveableDebuff != null)
				{
					if (other.transform.parent.parent.GetComponent<PlayerStats>().inflictedDebuff == null)
					{
						other.transform.parent.parent.GetComponent<PlayerStats>().inflictedDebuff = new Debuff
						{
							debuffName = playerOwner.giveableDebuff.name,
							debuffDuration = playerOwner.giveableDebuff.debuffDuration,
							damageInterval = playerOwner.giveableDebuff.damageInterval,
							damage = playerOwner.giveableDebuff.damage,
							shouldKill = playerOwner.giveableDebuff.shouldKill
						};

					}
				}
				other.transform.parent.parent.GetComponent<PlayerStats>().TakeDamage(damage);
			}
		}
		else if (other.CompareTag("StageBreakable"))
		{
			//Do damage to this stage block's "health" component.
			//TO DO: Have stage blocks that are breakable, with health component.
		}
	}	
	public void StartExpansion()
	{
		transform.localScale = new Vector3(minScale, minScale, minScale);
		StartCoroutine(Expand());
	}

	/// <summary>
	/// This coroutine expands the blast radius from the minScale size to its maximum scale before the explosion effect dissipates.
	/// </summary>
	/// <returns></returns>
	private IEnumerator Expand()
	{
		while (gameObject.transform.localScale.x < maxScale)
		{
			float increaseAmount = explosionSpeed * Time.deltaTime;
			gameObject.transform.localScale += new Vector3(increaseAmount, increaseAmount, increaseAmount);
			yield return null;
		}
		Destroy(gameObject);
	}
}
