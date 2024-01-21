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
	[Foldout("Dependencies"), Tooltip("")]
	private Transform[] fragmentDirections;

	[SerializeField, Required]
	[Foldout("Dependencies"), Tooltip("")]
	private GameObject explosionRadius;

	[SerializeField, ReadOnly]
	[Foldout("Stats"), Tooltip("")]
	private int originalPlayerIndex;

	[SerializeField, ReadOnly]
	[Foldout("Stats"), Tooltip("")]
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
	[Foldout("Stats"), Tooltip(""), Range(0, 1f)]
	private Transform target;

	private void OnEnable()
	{
		StartCoroutine(LifetimeClock());
		if (playerOwner != null)
		{
			if (playerOwner.homingBullets)
				FindClosestPlayer();
		}
	}

	private void Update()
	{
		// make bullet move direction
		if (playerOwner.homingBullets)
		{
			if (target != null)
			{
				Vector3 direction = target.position - transform.position;
				direction.y += 2;
				Vector3 inaccurateDir = Vector3.Slerp(direction.normalized, Random.onUnitSphere, 1 - playerOwner.homingAccuracy);

				Quaternion toRotation = Quaternion.LookRotation(inaccurateDir, Vector3.up);
				bulletRootObject.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, playerOwner.homingBulletRotSpeed * Time.deltaTime);
			}
		}
		bulletRootObject.position += transform.forward * movementSpeed * Time.deltaTime;
	}

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
		Debug.Log(other.name);
		//Burn effect
		if (other.CompareTag("Player"))
		{
			if (other.transform.parent.parent.GetComponent<PlayerBody>().PlayerIndex != originalPlayerIndex)
			{
				Debug.Log("Player hit!");
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
				other.transform.parent.parent.GetComponent<PlayerStats>().TakeDamage(damageToDeal);
				if (isFragmentable) BulletObjectPoolManager._Instance.ExpiredBullet(bulletRootObject.gameObject); 
				else Destroy(bulletRootObject.gameObject);
			}
		}
		//Else instead
		else if (other.CompareTag("StageNormal"))
		{
			Debug.Log(other.name);
			//If it does, check if player has fragmentation and if so, do this FIRST. Make sure the fragmented bullets don't fragment themselves!
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
			//Check if player has Unstable Blast, and if they do, explode!
			if (playerOwner.explodingBullets)
			{
				GameObject explosion = Instantiate(explosionRadius, transform.position, Quaternion.identity);
				explosion.GetComponent<Explosive>().playerOwner = this.playerOwner;
				explosion.GetComponent<Explosive>().originalPlayerIndex = this.originalPlayerIndex;
				explosion.GetComponent<Explosive>().StartExpansion();

			}
			if (isFragmentable) BulletObjectPoolManager._Instance.ExpiredBullet(bulletRootObject.gameObject);
			else Destroy(bulletRootObject.gameObject);
			return;
			
		}


	}

	public void ResetPlayerOwner(int newIndex, PlayerStats stats) { originalPlayerIndex = newIndex; playerOwner = stats; }
}
