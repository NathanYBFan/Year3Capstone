using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.VFX;

public class BulletBehaviour : MonoBehaviour
{
	#region Serialize Fields
	[SerializeField, Required]
	[Foldout("Dependencies"), Tooltip("")]
	private GameObject bulletGO;
	[SerializeField, Required]
	[Foldout("Dependencies"), Tooltip("")]
	private GameObject bulletShattered;
	[SerializeField, Required]
	[Foldout("Dependencies"), Tooltip("")]
	private GameObject burnEffect;
	[SerializeField, Required]
	[Foldout("Dependencies"), Tooltip("")]
	private Transform bulletRootObject;
	[SerializeField, Required]
	[Foldout("Dependencies"), Tooltip("The positions/angles of which the Fragmentation bullets will shoot in.")]
	private Transform[] fragmentDirections;
	[SerializeField, Required]
	[Foldout("Dependencies"), Tooltip("The explosion prefab for the Unstable Blast to spawn.")]
	private GameObject explosionRadius;

	[SerializeField, ReadOnly]
	[Foldout("Stats"), Tooltip("The index of the player who shot this bullet.")]
	private int originalPlayerIndex;

	[SerializeField, ReadOnly]
	[Foldout("Stats"), Tooltip("The stats of the player who shot this bullet.")]
	private PlayerStats playerOwner;

	[SerializeField]
	[Foldout("Stats"), Tooltip("The stats of the player who shot this bullet.")]
	private TrailRenderer trailMat;

	[SerializeField]
	[Foldout("Stats"), Tooltip("")]
	private float lifeTime = 5f;

	[SerializeField]
	[Foldout("Stats"), Tooltip("")]
	private float movementSpeed = 2.5f;

	[SerializeField]
	[Foldout("Stats"), Tooltip("")]
	public bool isFragmentable = true;

	[SerializeField]
	[Foldout("Stats"), Tooltip("")]
	private Transform target;
	#endregion Serialize Fields

	private void OnEnable()
	{
		if (playerOwner != null)
		{
			// We want the bullets to get a target as soon as they are used from the object pool, if the player has homing bullets enabled.
			if (playerOwner.HomingBullets)
				FindClosestPlayer();
			if (playerOwner.ExplodingBullets || playerOwner.HomingBullets)
				lifeTime = 2f;
			else lifeTime = 5f;
			if (playerOwner.GiveableDebuff != null)
			{
				burnEffect.SetActive(true);
			}
			else
			{
				burnEffect.SetActive(false);
			}
		}
		StartCoroutine(LifetimeClock());
	}

	private void Update()
	{
		// Assign the new materials array back to the renderer

		/*if (burnEffect.activeSelf)
		{
			burnEffect.transform.Rotate(0, 720 * Time.deltaTime, 0);
		}*/


		if (playerOwner.HomingBullets)
		{
			// If these bullets are to be homing bullets, then their direction will be altered to aim towards a defined target.
			if (target != null)
			{
				Vector3 direction = target.position - transform.position;
				direction.y += 2; // Centers bullet to player body (roughly).

				Vector3 inaccurateDir = Vector3.Slerp(direction.normalized, Random.onUnitSphere, 1 - playerOwner.HomingAccuracy);
				Quaternion toRotation = Quaternion.LookRotation(inaccurateDir, Vector3.up);
				bulletRootObject.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, playerOwner.HomingBulletRotSpeed * Time.deltaTime);
				if (target.gameObject.GetComponent<PlayerStats>().IsDead) FindClosestPlayer();
			}
		}
		// Otherwise make bullet move default direction
		bulletRootObject.position += transform.forward * movementSpeed * Time.deltaTime;
	}

	private void OnTriggerEnter(Collider other)
	{
		GameObject bulletFX;
		switch (other.tag)
		{
			case "Shield":
				if (other.gameObject != playerOwner.gameObject.GetComponent<PlayerBody>().Shield)
				{
					bulletFX = Instantiate(bulletShattered, transform.position, Quaternion.identity);
					BulletObjectPoolManager._Instance.ObjectsToDispose.Add(bulletFX);
					if (isFragmentable) BulletObjectPoolManager._Instance.ExpiredBullet(bulletRootObject.gameObject);
					else
					{
						BulletObjectPoolManager._Instance.FragmentedBullets.Remove(bulletRootObject.gameObject);
						Destroy(bulletRootObject.gameObject);
					}
				}
				break;
			case "Player":
				if (other.transform.parent.parent.GetComponent<PlayerBody>().PlayerIndex != originalPlayerIndex)
				{
					bulletFX = Instantiate(bulletShattered, transform.position, Quaternion.identity);
					BulletObjectPoolManager._Instance.ObjectsToDispose.Add(bulletFX);
					// Check to see if these bullets should have a burn effect.
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
					// Deal damage.
					other.transform.parent.parent.GetComponent<PlayerStats>().TakeDamage(playerOwner.Damage, DamageType.Bullet);

					// If these bullets were instantiated from Fragmentation, then they should get destroyed. If not, then they can be readded to the bullet object pool.
					if (isFragmentable) BulletObjectPoolManager._Instance.ExpiredBullet(bulletRootObject.gameObject);
					else
					{
						BulletObjectPoolManager._Instance.FragmentedBullets.Remove(bulletRootObject.gameObject);
						Destroy(bulletRootObject.gameObject);
					}
				}
				break;
			case "StageNormal":
				bulletFX = Instantiate(bulletShattered, transform.position, Quaternion.identity);
				BulletObjectPoolManager._Instance.ObjectsToDispose.Add(bulletFX);
				// If the player has Fragmentation and the bullet that hit the stage is fragmentable, split the bullet!
				if (playerOwner.FragmentBullets && isFragmentable)
				{
					// Assigning fragmented bullets the stats that the parent bullet had.
					for (int i = 0; i < 3; i++)
					{
						GameObject bullet = Instantiate(bulletGO, fragmentDirections[i].position, Quaternion.identity, BulletObjectPoolManager._Instance.transform);
						BulletObjectPoolManager._Instance.FragmentedBullets.Add(bullet);
						bullet.GetComponentInChildren<BulletBehaviour>().playerOwner = this.playerOwner;
						bullet.GetComponentInChildren<BulletBehaviour>().originalPlayerIndex = this.originalPlayerIndex;
						bullet.GetComponentInChildren<BulletBehaviour>().isFragmentable = false;
						bullet.GetComponentInChildren<BulletBehaviour>().bulletRootObject = bullet.gameObject.transform;
						bullet.GetComponentInChildren<BulletBehaviour>().explosionRadius = this.explosionRadius;
						Vector3 bulletRot = bullet.transform.rotation.eulerAngles;
						bulletRot.y = fragmentDirections[i].rotation.eulerAngles.y;
						bullet.transform.rotation = Quaternion.Euler(bulletRot);
					}
				}
				// Check if player has Unstable Blast, and if they do, make this bullet explode!
				if (playerOwner.ExplodingBullets)
				{
					GameObject explosion = Instantiate(explosionRadius, transform.position, Quaternion.identity, BulletObjectPoolManager._Instance.transform);
					explosion.GetComponent<Explosive>().PlayerOwner = this.playerOwner;
					explosion.GetComponent<Explosive>().OriginalPlayerIndex = this.originalPlayerIndex;
					explosion.GetComponent<Explosive>().StartExpansion(false);
					BulletObjectPoolManager._Instance.ExplodedBullets.Add(explosion);

				}
				// If these bullets were instantiated from Fragmentation, then they should get destroyed. If not, then they can be readded to the bullet object pool.
				if (isFragmentable) BulletObjectPoolManager._Instance.ExpiredBullet(bulletRootObject.gameObject);
				else
				{
					BulletObjectPoolManager._Instance.FragmentedBullets.Remove(bulletRootObject.gameObject);
					Destroy(bulletRootObject.gameObject);
				}
				break;
			case "DestroysBullets":
				if (isFragmentable) BulletObjectPoolManager._Instance.ExpiredBullet(bulletRootObject.gameObject);
				else
				{
					BulletObjectPoolManager._Instance.FragmentedBullets.Remove(bulletRootObject.gameObject);
					Destroy(bulletRootObject.gameObject);
				}
				break;
			default:
				break;
		}
	}

	private void FindClosestPlayer()
	{
		float closestDistance = Mathf.Infinity;
		foreach (GameObject player in GameManager._Instance.Players)
		{
			if (player.GetComponent<PlayerBody>().PlayerIndex != originalPlayerIndex && !player.GetComponent<PlayerStats>().IsDead)
			{
				float distance = Vector3.Distance(transform.position, player.transform.position);
				if (distance < closestDistance)
				{
					closestDistance = distance;
					target = player.transform;
				}
			}
		}
	}

	private IEnumerator LifetimeClock()
	{
		yield return new WaitForSeconds(lifeTime);
		if (playerOwner.ExplodingBullets)
		{
			GameObject explosion = Instantiate(explosionRadius, transform.position, Quaternion.identity, BulletObjectPoolManager._Instance.transform);
			explosion.GetComponent<Explosive>().PlayerOwner = this.playerOwner;
			explosion.GetComponent<Explosive>().OriginalPlayerIndex = this.originalPlayerIndex;
			explosion.GetComponent<Explosive>().StartExpansion(false);
			BulletObjectPoolManager._Instance.ExplodedBullets.Add(explosion);
		}
		BulletObjectPoolManager._Instance.ExpiredBullet(bulletRootObject.gameObject);
		yield break;
	}

	public void ResetPlayerOwner(int newIndex, PlayerStats stats)
	{
		originalPlayerIndex = newIndex;
		playerOwner = stats;
		trailMat.material = playerOwner.PlayerBulletMaterial;
	}
}
