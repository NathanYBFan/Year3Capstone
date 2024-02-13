using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPoolManager : MonoBehaviour
{
	// Singleton Initialization
	public static BulletObjectPoolManager _Instance;

    #region SerializeFields
    [SerializeField, ReadOnly]
	[Foldout("Dependencies"), Tooltip("List of bullets that are deactivated and not in play")]
	private List<GameObject> deactivatedBullets;

	[SerializeField, ReadOnly]
	[Foldout("Dependencies"), Tooltip("List of bullets that are actively being used")]
	private List<GameObject> activatedBullets;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("Default prefab of a bullet to use")]
	private GameObject defaultBullet;

	[SerializeField]
	[Foldout("Stats"), Tooltip("The total number of bullets to fill the object pool with on start")]
	private int totalBulletsInPool = 50;

	[SerializeField]
	[Foldout("Stats"), Tooltip("Toggle if bullet object pool should be able to grow")]
	private bool bulletPoolCanGrow = false;

	[SerializeField]
	[Foldout("Stats"), Tooltip("Absolute MAX bullets allowed to grow to")]
	private int hardCapBulletCount = 100;
    #endregion

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
	}

	// Fills the selected list with bullets of a type
	private void PropogateList(List<GameObject> bulletList, GameObject bulletType)
	{
		for (int i = 0; i < totalBulletsInPool; i++)
		{
			GameObject bullet = GameObject.Instantiate(bulletType, transform);
			bullet.SetActive(false);
			deactivatedBullets.Add(bullet);
		}
	}

	// If a bullet is fired
	public GameObject FiredBullet()
	{
		GameObject bulletToReturn = null;

		if (deactivatedBullets.Count == 0) // If the unused bullet pool is empty
		{
			if (!bulletPoolCanGrow) return null; // Pool shouldnt grow, then do nothing

			if (totalBulletsInPool >= hardCapBulletCount) // If the absolute max cap has been reached
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

		return bulletToReturn; // Return fired bullet
	}

	// If a bullet is finished its lifespan (Hit object or times out)
	public void ExpiredBullet(GameObject bullet)
	{
		if (!bullet.GetComponentInChildren<BulletBehaviour>().isFragmentable) Destroy(bullet);
        
		// Deactivate the bullet
        bullet.SetActive(false);
		// Filter into correct list
		deactivatedBullets.Add(bullet);
		activatedBullets.Remove(bullet);
	}

	// Deactivates all bullets
	public void ResetAllBullets()
	{
		foreach (GameObject bullet in activatedBullets)
			ExpiredBullet(bullet);
	}
}
