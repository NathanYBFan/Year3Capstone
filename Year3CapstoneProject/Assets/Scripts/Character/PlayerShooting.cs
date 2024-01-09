using NaughtyAttributes;
using UnityEngine;

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
    [Foldout("Stats"), Tooltip("")]
    private float shootingCooldown;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private int playerNumber;

    // Fire Bullet Method
    public void FireBullet()
    {
        // TODO NATHANF: MAKE OBJECT POOL FOR BULLETS
        GameObject bullet = BulletObjectPoolManager._Instance.FiredBullet();
        // set bullet visual to bulletDisplay
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.SetActive(true);

        // AudioManager._Instance.PlaySoundFX(playerAudioList[i], playerAudioSourceList[playerNumber]);
    }
}
