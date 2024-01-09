using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosFactorManager : MonoBehaviour
{
    // Singleton Initialization
    public static ChaosFactorManager _Instance;

    // Serialize Fields
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private List<string> chaosFactorList;

    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("")]
    private float nextChaosFactorTimer;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private float chaosFactorMaxTimer;


    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Debug.LogWarning("Destroyed a repeated ChaosFactorManager");
            Destroy(this.gameObject);
        }

        else if (_Instance == null)
            _Instance = this;
    }

    // Start Chaos Factor
    public void StartChaosFactor()
    {
        StartCoroutine(RunChaosFactor());
    }

    // Run Coroutine
    public IEnumerator RunChaosFactor()
    {
        yield return null;
    }
}
