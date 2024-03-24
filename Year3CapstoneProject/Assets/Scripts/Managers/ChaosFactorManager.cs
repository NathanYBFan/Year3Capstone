using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChaosFactorManager : MonoBehaviour
{
    // Singleton Initialization
    public static ChaosFactorManager _Instance;

    #region SerializeFields
    [SerializeField]
    private GameObject gameManagerRef;

    [SerializeField]
    private Image alert;

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
    public bool ChaosFactorActive { get { return chaosFactorActive; } set { chaosFactorActive = value; } }
    #endregion

    private bool timer;
    private bool chaosFactorActive = false;

    //private int maxRed;
    //private int maxGreen;
    //private int maxBlue;

    private int minRed = 100;

    private int pulseCount = 3;
    private void Awake()
    {
        timer = false;
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

        if (Input.GetKeyDown("[3]"))
        {
            Debug.Log("Input recived: Numpad 3");
            StartCoroutine(RunChaosFactor(chaosFactorList[2]));
        }

        if (Input.GetKeyDown("[4]"))
        {
            Debug.Log("Input recived: Numpad 4");
            StartCoroutine(RunChaosFactor(chaosFactorList[3]));
        }

        if (Input.GetKeyDown("[5]"))
        {
            Debug.Log("Input recived: Numpad 5");
            StartCoroutine(RunChaosFactor(chaosFactorList[4]));
        }

        if (Input.GetKeyDown("[6]"))
        {
            Debug.Log("Input recived: Numpad 6");
            StartCoroutine(RunChaosFactor(chaosFactorList[5]));
        }

        if (Input.GetKeyDown("[7]"))
        {
            Debug.Log("Input recived: Numpad 7");
            StartCoroutine(RunChaosFactor(chaosFactorList[6]));
        }

        if (Input.GetKeyDown("[9]"))
        {
            Debug.Log("Input recived: Numpad 9");
            timer = !timer;
            nextChaosFactorTimerSeconds = 30;
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
            else if (timer == true) { nextChaosFactorTimerSeconds += Time.deltaTime; }
                //

            yield return null;
        }
        yield break;
    }

    // Run Coroutine
    public IEnumerator RunChaosFactor(GameObject chaosFactorToSpawn)
    {


        yield return CFAlert();

        //play sounds/voice lines
        AudioClip clipToPlay = AudioManager._Instance.MRTwentyChaosFactorList[Random.Range(0,AudioManager._Instance.MRTwentyChaosFactorList.Count)];
        AudioManager._Instance.PlaySoundFX(clipToPlay, AudioManager._Instance.MRTwentyAudioSource);
        AudioManager._Instance.ResetInactivityTimer();

        chaosFactorActive = true;
        // Instantiate Chaos Factor
        GameObject chaosFactor = GameObject.Instantiate(chaosFactorToSpawn, transform);
        currentRunningChaosFactors.Add(chaosFactor);
        //StartCoroutine(DestroyAfterTime(chaosFactor, 20)); // Destroy Chaos Factor after 1 minute
        StartCoroutine(DestroyAfterTime(chaosFactor, chaosFactor.GetComponent<ChaosFactor>().Timer)); // Destroy Chaos Factor after 1 minute
       

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
		chaosFactorActive = false;
		currentRunningChaosFactors.Remove(chaosFactorToDestroy);
        Destroy(chaosFactorToDestroy);
    }

    public IEnumerator CFAlert()
    {
        alert.enabled = true;

        float speed = -10f;
        Color startColor = alert.color;
        Color endColor;
        endColor.r = 0;
        endColor.g = 0;
        endColor.b = 0;
        endColor.a = 255;
        Color lerpedColor = startColor;
        float tick = 0f;

        Color pulseEnd = endColor;
        pulseEnd.a = 0;

        for (int i = 0; i < pulseCount-1; i++)
        {

            while (alert.color != pulseEnd)
            {

                tick += Time.deltaTime;
                lerpedColor = Color.Lerp(startColor, pulseEnd, tick);
                alert.color = lerpedColor;



                if (true)
                {

                }
                yield return null;
            }

            tick = 0f;

            while (alert.color != startColor)
            {

                tick += Time.deltaTime;
                lerpedColor = Color.Lerp(pulseEnd, startColor, tick);
                alert.color = lerpedColor;



                if (true)
                {

                }
                yield return null;
            }

            tick = 0f;

        }

        while (alert.color != pulseEnd)
        {

            tick += Time.deltaTime;
            lerpedColor = Color.Lerp(startColor, pulseEnd, tick);
            alert.color = lerpedColor;



            if (true)
            {

            }
            yield return null;
        }


        alert.enabled = false;


        yield break;
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
        StopAllCoroutines();
        // Reset timer to 0
        ResetChaosFactorTimer();
        // Remove any active Chaos Factor
        RemoveAllChaosFactors();
    }
}
