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
    [Foldout("Stats"), Tooltip("The total number of bullets to fill the object pool with on start")]
    private int totalBulletsInPool = 50;

    [SerializeField]
    [Foldout("Stats"), Tooltip("Toggle if bullet object pool should be able to grow")]
    private bool bulletPoolCanGrow = false;

    [SerializeField]
    [Foldout("Stats"), Tooltip("Absolute MAX bullets allowed to grow to")]
    private int hardCapBulletCount = 100;

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
        for (int i = 0; i < totalBulletsInPool; i++)
        {
            GameObject bullet = GameObject.Instantiate(bulletType, transform);
            bullet.SetActive(false);
            deactivatedBullets.Add(bullet);
        }
    }

    public GameObject FiredBullet()
    {
        GameObject bulletToReturn = null;

        if (deactivatedBullets.Count == 0)
        {
            if (!bulletPoolCanGrow) return null;
         
            if (totalBulletsInPool >= hardCapBulletCount) // If max cap has been reached
            {
                bulletPoolCanGrow = false;
                return null;
            }
            // Create Bullet & Reset
            bulletToReturn = GameObject.Instantiate(defaultBullet, transform);
            bulletToReturn.SetActive(false);
            
            // Add to Correct Object Pool
            activatedBullets.Add(bulletToReturn);

            // Increase proper counts
            totalBulletsInPool++;

            return bulletToReturn;
        }
        // If there are still bullets in pool:
        bulletToReturn = deactivatedBullets[0];

        // Add and remove from poper lists:
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
    
    public void ResetAllBullets()
    {
        foreach(GameObject bullet in activatedBullets)
            ExpiredBullet(bullet);
    }
}
