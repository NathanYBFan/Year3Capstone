using System.Collections;
using UnityEngine;

public class DestroyAfterLifetime : MonoBehaviour
{
	[SerializeField]
	float lifeTime = 4;
	private void Awake()
	{
		StartCoroutine(LifetimeClock());
	}
	private IEnumerator LifetimeClock()
	{
		yield return new WaitForSeconds(lifeTime);
		Destroy(gameObject);
		yield break;
	}
}
