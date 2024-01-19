using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class PlayerShooting : MonoBehaviour
{
    // Serialize Fields
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject bulletDisplay;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private Transform firePoint;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private Transform playerRotation;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private float shootingCooldown;

    //[SerializeField]
    //[Foldout("Stats"), Tooltip("")]
    //private int playerNumber;

    // Fire Bullet Method
    public void FireBullet()
    {
        GameObject bullet = BulletObjectPoolManager._Instance.FiredBullet();
        // set bullet visual to bulletDisplay
        bullet.transform.position = firePoint.position;
        Vector3 bulletRot = bullet.transform.rotation.eulerAngles;
        bulletRot.y = playerRotation.rotation.eulerAngles.y;
        bullet.transform.rotation = Quaternion.Euler(bulletRot);

        bullet.GetComponentInChildren<BulletBehaviour>().ResetPlayerIndex(this.gameObject.GetComponent<PlayerBody>().PlayerIndex);
        
        bullet.SetActive(true);


        // TODO BRYAN: SETUP AUDIO HOOKUP + CODE;
        // AudioManager._Instance.PlaySoundFX(playerAudioList[i], playerAudioSourceList[playerNumber]);
    }
}
