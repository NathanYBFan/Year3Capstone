using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPoolManager : MonoBehaviour
{
    // Singleton Initialization
    public static BulletObjectPoolManager _Instance;

    [SerializeField, ReadOnly]
    [Foldout("Dependencies"), Tooltip("")]
    private List<GameObject> deactivatedBullets;

    [SerializeField, ReadOnly]
    [Foldout("Dependencies"), Tooltip("")]
    private List<GameObject> activatedBullets;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject defaultBullet;

    // If implimentation is greenlighted
    //[SerializeField, ReadOnly]
    //[Foldout("Dependencies"), Tooltip("")]
    //private List<GameObject> audienceBulletsDeactivated;
    //
    //[SerializeField]
    //[Foldout("Dependencies"), Tooltip("")]
    //private GameObject audienceBullet;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private int bulletsPerPlayer = 25;

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Debug.LogWarning("Destroyed a repeated BulletObjectPoolManager");
            Destroy(this.gameObject);
        }

        else if (_Instance == null)
            _Instance = this;
    }

    private void Start()
    {
        activatedBullets.Clear();
        PropogateList(deactivatedBullets, defaultBullet);

        // If audience shooting is implimented
        //PropogateList(audienceBulletsDeactivated, audienceBullet);
    }

    private void PropogateList(List<GameObject> bulletList, GameObject bulletType)
    {
        for (int i = 0; i < bulletsPerPlayer; i++)
        {
            GameObject bullet = GameObject.Instantiate(bulletType, transform);
            bullet.SetActive(false);
            deactivatedBullets.Add(bullet);
        }
    }

    public GameObject FiredBullet()
    {
        GameObject bulletToReturn = deactivatedBullets[0];
        deactivatedBullets.Remove(bulletToReturn);
        activatedBullets.Add(bulletToReturn);
        return bulletToReturn;
    }

    public void ExpiredBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        deactivatedBullets.Add(bullet);
        activatedBullets.Remove(bullet);
    }
}
