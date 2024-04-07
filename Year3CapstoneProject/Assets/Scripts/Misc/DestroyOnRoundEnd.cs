using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnRoundEnd : MonoBehaviour
{
	private void OnEnable()
	{
		GameManager.OnRoundEnd += OnDestroy;
	}
	private void OnDisable()
	{
		GameManager.OnRoundEnd -= OnDestroy;
	}

	private void OnDestroy()
	{
		Destroy(gameObject);
	}
}
