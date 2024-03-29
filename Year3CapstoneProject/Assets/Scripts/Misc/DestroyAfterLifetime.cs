using System.Collections;
using UnityEngine;

public class DestroyAfterLifetime : MonoBehaviour
{
	float lifeTime = 4;
	private void Awake()
	{
		StartCoroutine(LifetimeClock());
	}
	private IEnumerator LifetimeClock()
	{
		yield return new WaitForSeconds(lifeTime);
		BulletObjectPoolManager._Instance.ObjectsToDispose.Remove(gameObject);
		Destroy(gameObject);
		yield break;
	}
}
