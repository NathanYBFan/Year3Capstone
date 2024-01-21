using NaughtyAttributes;
using System.Collections;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
	// TODO NATHANF: FILL IN BEHAVIOURS

	// Serialize Fields
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
	[Foldout("Stats"), Tooltip("")]
	private float lifeTime = 5f;

	[SerializeField]
	[Foldout("Stats"), Tooltip("")]
	private float movementSpeed = 2.5f;

	[SerializeField]
	[Foldout("Stats"), Tooltip("")]
	private int damageToDeal = 1;

	[SerializeField]
	[Foldout("Stats"), Tooltip("")]
	public bool isFragmentable = true;

	[SerializeField]
	[Foldout("Stats"), Tooltip("")]
	private Transform target;

	private void OnEnable()
	{
		StartCoroutine(LifetimeClock());
		if (playerOwner != null)
		{
			//We want the bullets to get a target as soon as they are used from the object pool, if the player has homing bullets enabled.
			if (playerOwner.homingBullets)
				FindClosestPlayer();
		}
	}

	private void Update()
	{
		
		if (playerOwner.homingBullets)
		{
			//If these bullets are to be homing bullets, then their direction will be altered to aim towards a defined target.
			if (target != null)
			{
				Vector3 direction = target.position - transform.position;
				direction.y += 2; // To make the bullets shoot roughly from the center of the player's body, rather than at their feet.

				Vector3 inaccurateDir = Vector3.Slerp(direction.normalized, Random.onUnitSphere, 1 - playerOwner.homingAccuracy); //Adding innaccuracy here.

				Quaternion toRotation = Quaternion.LookRotation(inaccurateDir, Vector3.up);
				bulletRootObject.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, playerOwner.homingBulletRotSpeed * Time.deltaTime);
			}
		}
		//Otherwise make bullet move default direction
		bulletRootObject.position += transform.forward * movementSpeed * Time.deltaTime;
	}

	/// <summary>
	/// This helper method will locate the closest player to the player who shot this bullet (that is not themselves).
	/// </summary>
	private void FindClosestPlayer()
	{
		float closestDistance = Mathf.Infinity;
		foreach (GameObject player in GameManager._Instance.Players)
		{
			if (player.GetComponent<PlayerBody>().PlayerIndex != originalPlayerIndex)
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
		BulletObjectPoolManager._Instance.ExpiredBullet(bulletRootObject.gameObject);
		yield break;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (other.transform.parent.parent.GetComponent<PlayerBody>().PlayerIndex != originalPlayerIndex)
			{
				//Check to see if these bullets should have a burn effect.
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
				//Deal damage.
				other.transform.parent.parent.GetComponent<PlayerStats>().TakeDamage(damageToDeal);

				//If these bullets were instantiated from Fragmentation, then they should get destroyed. If not, then they can be readded to the bullet object pool.
				if (isFragmentable) BulletObjectPoolManager._Instance.ExpiredBullet(bulletRootObject.gameObject); 
				else Destroy(bulletRootObject.gameObject);
			}
		}

		else if (other.CompareTag("StageNormal") || other.CompareTag("StageBreakable"))
		{
			//If the player has Fragmentation and the bullet that hit the stage is fragmentable, split the bullet!
			if (playerOwner.fragmentBullets && isFragmentable)
			{
				for (int i = 0; i < 3; i++)
				{
					GameObject bullet = Instantiate(gameObject.transform.parent.gameObject, fragmentDirections[i].position, Quaternion.identity);
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
			//Check if player has Unstable Blast, and if they do, make this bullet explode!
			if (playerOwner.explodingBullets)
			{
				GameObject explosion = Instantiate(explosionRadius, transform.position, Quaternion.identity);
				explosion.GetComponent<Explosive>().playerOwner = this.playerOwner;
				explosion.GetComponent<Explosive>().originalPlayerIndex = this.originalPlayerIndex;
				explosion.GetComponent<Explosive>().StartExpansion();

			}

			//If these bullets were instantiated from Fragmentation, then they should get destroyed. If not, then they can be readded to the bullet object pool.
			if (isFragmentable) BulletObjectPoolManager._Instance.ExpiredBullet(bulletRootObject.gameObject);
			else Destroy(bulletRootObject.gameObject);
			return;
			
		}


	}

	public void ResetPlayerOwner(int newIndex, PlayerStats stats) { originalPlayerIndex = newIndex; playerOwner = stats; }
}
