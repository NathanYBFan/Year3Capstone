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

    private void Update()
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
        // Instantiate Chaos Factor
        GameObject chaosFactor = GameObject.Instantiate(chaosFactorToSpawn, transform);
        Destroy(chaosFactor, 60); // Destroy Chaos Factor after 1 minute

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

    public void ResetChaosFactorTimer() { nextChaosFactorTimerSeconds = 0f; }

    public void StartChaosFactorTest(int toTest) // TODO NATHANF: DEBUG TO BE REMOVED
    {
        Debug.Log("Made it to chaos factor manager test function, starting the coroutine, to test is: " + toTest);
            
        StartCoroutine(RunChaosFactor(chaosFactorList[toTest]));
            
        ResetChaosFactorTimer();
    }

    public void Reset() // TODO NATHANF: FILL THIS IN
    {
        // Reset timer
        // Remove any active Chaos Factor
    }
}
