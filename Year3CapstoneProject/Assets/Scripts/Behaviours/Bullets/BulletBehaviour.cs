using NaughtyAttributes;
using System.Collections;
using TreeEditor;
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
	private bool isFragmentable = true;

	private void OnEnable()
	{
		StartCoroutine(LifetimeClock());
	}

	private void Update()
	{
		// make bullet move direction
		bulletRootObject.position += transform.forward * movementSpeed * Time.deltaTime;
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
					if (other.GetComponent<PlayerStats>().inflictedDebuff == null)
					{
						other.GetComponent<PlayerStats>().inflictedDebuff = new Debuff
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
					GameObject bullet = Instantiate(gameObject, fragmentDirections[i].position, Quaternion.identity);
					bullet.GetComponent<BulletBehaviour>().playerOwner = this.playerOwner;
					bullet.GetComponent<BulletBehaviour>().originalPlayerIndex = this.originalPlayerIndex;
					bullet.GetComponent<BulletBehaviour>().isFragmentable = false;
					bullet.GetComponent<BulletBehaviour>().bulletRootObject = bullet.gameObject.transform.parent;
					Vector3 bulletRot = bullet.transform.rotation.eulerAngles;
					bulletRot.y = fragmentDirections[i].rotation.eulerAngles.y;
					bullet.transform.rotation = Quaternion.Euler(bulletRot);
				}
			}

			BulletObjectPoolManager._Instance.ExpiredBullet(bulletRootObject.gameObject);
			return;
			//Check if player has Unstable Blast, and if they do, explode!
		}


	}

	public void ResetPlayerOwner(int newIndex, PlayerStats stats) { originalPlayerIndex = newIndex; playerOwner = stats; }
}
