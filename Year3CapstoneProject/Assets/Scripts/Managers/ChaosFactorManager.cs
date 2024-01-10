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
    [Foldout("Dependencies"), Tooltip("List of all Chaos Factors that can spawn in the game")]
    private List<GameObject> chaosFactorList;

    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("")]
    private float nextChaosFactorTimerSeconds = 0f;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private float chaosFactorMaxTimerSeconds = 30f;


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

    private void Start()
    {
        nextChaosFactorTimerSeconds = 0f;
    }

    // Start Chaos Factor
    public void StartChaosFactor()
    {
        while (GameManager._Instance.inGame)
        {
            if (nextChaosFactorTimerSeconds > chaosFactorMaxTimerSeconds)
            {
                int chaosFactorToSpawn = Random.Range(1, chaosFactorList.Count - 1);
                StartCoroutine(RunChaosFactor(chaosFactorList[chaosFactorToSpawn]));
                ResetChaosFactorTimer();
            }
            else
                nextChaosFactorTimerSeconds += Time.deltaTime;
        }
    }

    // Run Coroutine
    public IEnumerator RunChaosFactor(GameObject chaosFactorToSpawn)
    {
        GameObject chaosFactor = GameObject.Instantiate(chaosFactorToSpawn, transform);
        Destroy(chaosFactor, 60); // Destroy Chaos Factor after 1 minute
        
        yield return new WaitForSeconds(20); // Wait for 20 seconds
        ResetChaosFactorTimer();

        yield return new WaitForSeconds(20); // Wait for 20 seconds
        ResetChaosFactorTimer();

        yield return new WaitForSeconds(20); // Wait for 20 seconds
        ResetChaosFactorTimer();

        // After waiting for 1 minute in total,
        //ResetStage(); // Reset the stage: Lights, environment, etc.
        ResetChaosFactorTimer();

        yield break;
    }

    public void ResetChaosFactorTimer() { nextChaosFactorTimerSeconds = 0f; }
}
