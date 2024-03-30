using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerShooting : MonoBehaviour
{
	#region Serialize Fields
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]	private GameObject bulletDisplay;
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]	private Transform[] firePoint;
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]	private Transform playerRotation;
	#endregion Serialize Fields

	public void FireBullet()
	{
		// Set bullet visual to bulletDisplay
		PlayerStats stats = this.gameObject.GetComponent<PlayerStats>();
		if (!stats.CanShoot) return;
		
		if (stats.TriShot)
		{
			// They have Trifecta
			for (int i = 0; i < firePoint.Length; i++)
			{
				GameObject bullet = BulletObjectPoolManager._Instance.FiredBullet();
				bullet.transform.position = firePoint[i].position;
				bullet.transform.rotation = Quaternion.LookRotation(firePoint[i].forward);
                bullet.GetComponentInChildren<BulletBehaviour>().ResetPlayerOwner(this.gameObject.GetComponent<PlayerBody>().PlayerIndex, this.gameObject.GetComponent<PlayerStats>());
				bullet.SetActive(true);
			}
		}		
		else
		{
			// They don't have Trifecta
			GameObject bullet = BulletObjectPoolManager._Instance.FiredBullet();
			bullet.transform.position = firePoint[0].position;
			Vector3 bulletRot = bullet.transform.rotation.eulerAngles;
			bulletRot.y = playerRotation.rotation.eulerAngles.y;
			bullet.transform.rotation = Quaternion.Euler(bulletRot);
			bullet.GetComponentInChildren<BulletBehaviour>().ResetPlayerOwner(this.gameObject.GetComponent<PlayerBody>().PlayerIndex, this.gameObject.GetComponent<PlayerStats>());
			bullet.SetActive(true);
		}

		int playerIndex = GetComponent<PlayerBody>().PlayerIndex;
		if (AudioManager._Instance.PlayerAudioSourceList.Count > playerIndex || AudioManager._Instance.PlayerAudioList.Count > 0)
	        AudioManager._Instance.PlaySoundFX(AudioManager._Instance.PlayerAudioList[0], AudioManager._Instance.PlayerAudioSourceList[playerIndex]);
	}
}
