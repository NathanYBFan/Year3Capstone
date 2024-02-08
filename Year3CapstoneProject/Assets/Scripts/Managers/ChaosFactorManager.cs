using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosFactorManager : MonoBehaviour
{
    // Singleton Initialization
    public static ChaosFactorManager _Instance;

    #region SerializeFields
    [SerializeField]
    private GameObject gameManagerRef;

    [SerializeField]
    [Tooltip("List of all Chaos Factors that can spawn in the game")]
    private List<GameObject> chaosFactorList;

    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("Time until next chaos factor")]
    private float nextChaosFactorTimerSeconds = 0f;

    [SerializeField]
    [Foldout("Stats"), Tooltip("Max time between each chaos factor")]
    private float chaosFactorMaxTimerSeconds = 30f;

    [SerializeField]
    [Foldout("Stats"), Tooltip("A list of all chaos factors currently in play")]
    private List<GameObject> currentRunningChaosFactors;

    #endregion

    #region Getters&Setters
    public List<GameObject> ChaosFactorList { get { return chaosFactorList; } }
    #endregion

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Debug.LogWarning("Destroyed a repeated ChaosFactorManager");
            Destroy(this.gameObject);
        }

        else if (_Instance == null)
            _Instance = this;

        nextChaosFactorTimerSeconds = 0f;
    }

    private void Update() // TODO: DEBUG TO BE REMOVED
    {
        if (Input.GetKeyDown("[1]"))
        {
            Debug.Log("Input recived: Numpad 1");
            StartCoroutine(RunChaosFactor(chaosFactorList[0]));
        }
        if (Input.GetKeyDown("[2]"))
        {
            Debug.Log("Input recived: Numpad 2");
            StartCoroutine(RunChaosFactor(chaosFactorList[1]));
        }
        if (Input.GetKeyDown(KeyCode.O))
        {

        }
    }

    // Start Chaos Factor
    public void StartChaosFactor()
    {
        StartCoroutine(ChaosFactorSpawnTimer());
    }

    public IEnumerator ChaosFactorSpawnTimer()
    {
        while (GameManager._Instance.InGame)
        {
            if (nextChaosFactorTimerSeconds > chaosFactorMaxTimerSeconds)
            {
                int chaosFactorToSpawn = Random.Range(1, chaosFactorList.Count - 1);
                StartCoroutine(RunChaosFactor(chaosFactorList[chaosFactorToSpawn]));
                ResetChaosFactorTimer();
            }
            else
                nextChaosFactorTimerSeconds += Time.deltaTime;

            yield return null;
        }
        yield break;
    }

    // Run Coroutine
    public IEnumerator RunChaosFactor(GameObject chaosFactorToSpawn)
    {
        // Instantiate Chaos Factor
        GameObject chaosFactor = GameObject.Instantiate(chaosFactorToSpawn, transform);
        currentRunningChaosFactors.Add(chaosFactor);
        StartCoroutine(DestroyAfterTime(chaosFactor, 60)); // Destroy Chaos Factor after 1 minute

        yield return new WaitForSeconds(20); // Wait for 20 seconds <-- These should be tuneable
        ResetChaosFactorTimer();

        yield return new WaitForSeconds(20); // Wait for 20 seconds <-- These should be tuneable
        ResetChaosFactorTimer();

        yield return new WaitForSeconds(20); // Wait for 20 seconds <-- These should be tuneable
        ResetChaosFactorTimer();

        // After waiting for 1 minute in total,
        //ResetStage(); // Reset the stage: Lights, environment, etc.
        ResetChaosFactorTimer();

        yield break;
    }

    public IEnumerator DestroyAfterTime(GameObject chaosFactorToDestroy, float timeToWaitFor)
    {
        yield return new WaitForSeconds(timeToWaitFor);
        currentRunningChaosFactors.Remove(chaosFactorToDestroy);
        Destroy(chaosFactorToDestroy);
    }

    public void ResetChaosFactorTimer() { nextChaosFactorTimerSeconds = 0f; }

    private void RemoveAllChaosFactors()
    {
        StopAllCoroutines();
        for (int i = 0; i < currentRunningChaosFactors.Count; i++)
        {
            Destroy(currentRunningChaosFactors[i]);
        }
        currentRunningChaosFactors.Clear();
    }

    // Public method to completely reset all chaos factors
    public void Reset()
    {
        // Reset timer to 0
        ResetChaosFactorTimer();
        // Remove any active Chaos Factor
        RemoveAllChaosFactors();
    }
}
