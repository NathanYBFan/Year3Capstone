using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerShooting : MonoBehaviour
{
	// Serialize Fields
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private GameObject bulletDisplay;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private Transform[] firePoint;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private Transform playerRotation;


	//[SerializeField]
	//[Foldout("Stats"), Tooltip("")]
	//private int playerNumber;

	// Fire Bullet Method
	public void FireBullet()
	{

		// set bullet visual to bulletDisplay
		PlayerStats stats = this.gameObject.GetComponent<PlayerStats>();

		//They have Trifecta
		if (stats.triShot)
		{
			for (int i = 0; i < firePoint.Length; i++)
			{
				GameObject bullet = BulletObjectPoolManager._Instance.FiredBullet();
				bullet.transform.position = firePoint[i].position;
				Vector3 bulletRot = bullet.transform.rotation.eulerAngles;
				bulletRot.y = playerRotation.rotation.eulerAngles.y + firePoint[i].rotation.eulerAngles.y;
				bullet.transform.rotation = Quaternion.Euler(bulletRot);

				bullet.GetComponentInChildren<BulletBehaviour>().ResetPlayerOwner(this.gameObject.GetComponent<PlayerBody>().PlayerIndex, this.gameObject.GetComponent<PlayerStats>());

				bullet.SetActive(true);
			}
		}
		//They don't have Trifecta
		else
		{
			GameObject bullet = BulletObjectPoolManager._Instance.FiredBullet();
			bullet.transform.position = firePoint[0].position;
			Vector3 bulletRot = bullet.transform.rotation.eulerAngles;
			bulletRot.y = playerRotation.rotation.eulerAngles.y;
			bullet.transform.rotation = Quaternion.Euler(bulletRot);

			bullet.GetComponentInChildren<BulletBehaviour>().ResetPlayerOwner(this.gameObject.GetComponent<PlayerBody>().PlayerIndex, this.gameObject.GetComponent<PlayerStats>());

			bullet.SetActive(true);
		}
		int playerIndex = GetComponent<PlayerBody>().PlayerIndex;
        AudioManager._Instance.PlaySoundFX(AudioManager._Instance.PlayerAudioList[0], AudioManager._Instance.PlayerAudioSourceList[playerIndex]);
	}
}
