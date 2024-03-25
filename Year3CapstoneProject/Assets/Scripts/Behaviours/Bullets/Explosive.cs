using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public enum DamageType
{
	Bullet,
	Hazard,
	Explosive,
	ChaosFactor,
	Falling,
	Lethal,
	none
}
public class Explosive : MonoBehaviour
{
	#region Serialize Fields
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("Explosion particle")]
	private ParticleSystem explosion;

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
	[SerializeField]
	[Foldout("Stats"), Tooltip("The delay (in seconds) until the explosion actually starts.")]
	private float explosionDelay;

	[SerializeField, ReadOnly]
	[Foldout("Stats"), Tooltip("")]
	private int originalPlayerIndex;

	[SerializeField, ReadOnly]
	[Foldout("Stats"), Tooltip("")]
	private PlayerStats playerOwner;

	#endregion Serialize Fields
	#region Getters & Setters
	public int OriginalPlayerIndex {  get { return originalPlayerIndex; } set {  originalPlayerIndex = value; } }
	public PlayerStats PlayerOwner { get {  return playerOwner; } set {  playerOwner = value; } }
	public ParticleSystem Explosion {  get { return explosion; } set {  explosion = value; } }
	#endregion Getters & Setters

	private void OnTriggerEnter(Collider other)
	{
		switch (other.tag)
		{
			case "Player":
				// Burn effect.
				if (other.transform.parent.parent.GetComponent<PlayerBody>().PlayerIndex != originalPlayerIndex)
				{
					if (playerOwner.GiveableDebuff != null)
					{
						if (other.transform.parent.parent.GetComponent<PlayerStats>().InflictedDebuff == null)
						{
							other.transform.parent.parent.GetComponent<PlayerStats>().InflictedDebuff = new Debuff
							{
								debuffName = playerOwner.GiveableDebuff.name,
								debuffDuration = playerOwner.GiveableDebuff.debuffDuration,
								damageInterval = playerOwner.GiveableDebuff.damageInterval,
								damage = playerOwner.GiveableDebuff.damage,
								shouldKill = playerOwner.GiveableDebuff.shouldKill
							};

						}
					}
					other.transform.parent.parent.GetComponent<PlayerStats>().TakeDamage(damage, DamageType.Explosive);
				}
				break;
			case "StageNormal":
				//Do damage to this stage block's "health" component.
				//TO DO: Have stage blocks that are breakable, with health component.
				break;
			default: break;
		}
	}	

	public void StartExpansion(bool isSelfDestruct)
	{
		transform.localScale = new Vector3(minScale, minScale, minScale);
		StartCoroutine(Expand(isSelfDestruct));
	}

	/// <summary>
	/// This coroutine expands the blast radius from the minScale size to its maximum scale before the explosion effect dissipates.
	/// </summary>
	/// <returns></returns>
	private IEnumerator Expand(bool isSelfDestruct)
	{
        yield return new WaitForSeconds(explosionDelay);
		while (gameObject.transform.localScale.x < maxScale)
		{
			explosion.Play();
			float increaseAmount = explosionSpeed * Time.deltaTime;
			gameObject.transform.localScale += new Vector3(increaseAmount, increaseAmount, increaseAmount);
			yield return null;
		}
		if (isSelfDestruct)
		{
			playerOwner.TakeDamage(9999999, DamageType.Explosive);
			yield return new WaitForSeconds(0.01f);
		}
		BulletObjectPoolManager._Instance.ExplodedBullets.Remove(gameObject);
		explosion.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		Destroy(gameObject);
	}
}
