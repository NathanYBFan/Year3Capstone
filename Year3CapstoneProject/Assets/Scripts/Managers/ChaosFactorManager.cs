using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private bool timer;

    [SerializeField]
    private bool demoMode;

    [SerializeField] 
	private TextMeshProUGUI alertText;

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

	public List<GameObject> CurrentRunningChaosFactors { get { return currentRunningChaosFactors; } set { currentRunningChaosFactors = value; } }



	#endregion

	#region Getters&Setters
	public List<GameObject> ChaosFactorList { get { return chaosFactorList; } }
	public bool ChaosFactorActive { get { return chaosFactorActive; } set { chaosFactorActive = value; } }
	#endregion

	
	private bool chaosFactorActive = true;



	private Color startColor;
	private Color textStart;

	public int activeCFCount;

	[SerializeField]
	private int[] demoOrder;

	private int pulseCount = 4;

	private int demoCount;

    private int demoTracker = 0;

	private void Awake()
	{
		demoCount = demoOrder.Length-1;

		startColor = Color.white;

		activeCFCount = 0;


        alertText.color = Color.red;

		//timer = true;
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

        if (Input.GetKeyDown("[8]"))
        {
			Debug.Log("Input recived: Numpad 8");
            timer = true;
            nextChaosFactorTimerSeconds = 0;

            demoMode = !demoMode;
			demoTracker = 0;
        }

        if (Input.GetKeyDown("[9]"))
        {
            Debug.Log("Input recived: Numpad 9");
            timer = !timer;
            nextChaosFactorTimerSeconds = 0;
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
        {	//debug if to delete
            if (activeCFCount > 2)
            {
                print("It's broken");
            }

            if (nextChaosFactorTimerSeconds > chaosFactorMaxTimerSeconds)
			{
				print("timer over");
				if (activeCFCount >= 2)
				{
					float shortestTimer = transform.GetChild(0).GetComponent<ChaosFactor>().Timer;
					if (shortestTimer > transform.GetChild(1).GetComponent<ChaosFactor>().Timer) { shortestTimer = transform.GetChild(1).GetComponent<ChaosFactor>().Timer; }                   
                    yield return new WaitForSeconds(shortestTimer);
                }

				if (demoMode == false)
				{
                    if (activeCFCount == 1)
                    {

                        int chaosFactorToSpawn = Random.Range(0, GetComponentInChildren<ChaosFactor>().CompatibleCFs.Length);
                        StartCoroutine(RunChaosFactor(GetComponentInChildren<ChaosFactor>().CompatibleCFs[chaosFactorToSpawn]));
                        ResetChaosFactorTimer();
                    }
                    else
                    {
                        int chaosFactorToSpawn = Random.Range(0, chaosFactorList.Count);
                        StartCoroutine(RunChaosFactor(chaosFactorList[chaosFactorToSpawn]));
                        ResetChaosFactorTimer();
                    }
                }

				else if (demoMode == true)
				{
					print("Entered demomode if");
                    StartCoroutine(RunChaosFactor(chaosFactorList[demoOrder[demoTracker]]));
                    ResetChaosFactorTimer();
                    demoTracker++;
					if (demoTracker > demoCount)
					{ 
						demoTracker = 0;
					}
                }
				

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

		alert.enabled = true;
		yield return CFAlert(chaosFactorToSpawn);

		//play sounds/voice lines
		AudioClip clipToPlay = AudioManager._Instance.MRTwentyChaosFactorList[Random.Range(0, AudioManager._Instance.MRTwentyChaosFactorList.Count)];
		AudioManager._Instance.PlaySoundFX(clipToPlay, AudioManager._Instance.MRTwentyAudioSource);
		AudioManager._Instance.ResetInactivityTimer();

		chaosFactorActive = true;
		// Instantiate Chaos Factor
		GameObject chaosFactor = GameObject.Instantiate(chaosFactorToSpawn, transform);
		currentRunningChaosFactors.Add(chaosFactor);
		//StartCoroutine(DestroyAfterTime(chaosFactor, 20)); // Destroy Chaos Factor after 1 minute
		StartCoroutine(DestroyAfterTime(chaosFactor, chaosFactor.GetComponent<ChaosFactor>().Timer)); // Destroy Chaos Factor after 1 minute


		//yield return new WaitForSeconds(20); // Wait for 20 seconds <-- These should be tuneable
		//ResetChaosFactorTimer();

		//yield return new WaitForSeconds(20); // Wait for 20 seconds <-- These should be tuneable
		//ResetChaosFactorTimer();

		//yield return new WaitForSeconds(20); // Wait for 20 seconds <-- These should be tuneable
		//ResetChaosFactorTimer();

		// After waiting for 1 minute in total,
		//ResetStage(); // Reset the stage: Lights, environment, etc.
		//ResetChaosFactorTimer();

		yield break;
	}

	public IEnumerator DestroyAfterTime(GameObject chaosFactorToDestroy, float timeToWaitFor)
	{
		yield return new WaitForSeconds(timeToWaitFor);
		chaosFactorActive = false;
		if (chaosFactorToDestroy == null) yield break;
		currentRunningChaosFactors.Remove(chaosFactorToDestroy);
		chaosFactorToDestroy.GetComponent<ChaosFactor>().OnEndOfChaosFactor(false);
	}

	public IEnumerator CFAlert(GameObject CF)
	{
		GameObject.Find("Mr.20").GetComponent<MoveMr20>().InitiateChaosFactor();
		alert.enabled = true;

		alertText.text = "Chaos Factor Incoming";

		Color endColor = new Color(0, 0, 0, 255); ;

		Color lerpedColor = startColor;
		float tick = 0f;

		Color pulseEnd = endColor;
		pulseEnd.a = 0;


        textStart = new Color(1, 0, 0, 0.0f);

        Color TextEnd = new Color(1, 0, 0, 1);

		Color textLerp;

		Color textPulse = TextEnd;
		




		for (int i = 0; i < pulseCount - 1; i++)
		{

			while (alert.color != pulseEnd)
			{

				tick += Time.deltaTime;

				lerpedColor = Color.Lerp(startColor, pulseEnd, tick);
				alert.color = lerpedColor;

				textLerp = Color.Lerp(textStart, textPulse, tick);
				alertText.color = textLerp;

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


                textLerp = Color.Lerp(textPulse, textStart, tick);
                alertText.color = textLerp;

                if (true)
				{

				}
				yield return null;
			}

			tick = 0f;

			if (i == 1)
			{
                alertText.text = CF.GetComponent<ChaosFactor>().Name;

            }



		}

		while (alert.color != pulseEnd)
		{

			tick += Time.deltaTime;
			lerpedColor = Color.Lerp(startColor, pulseEnd, tick);
			alert.color = lerpedColor;

            textLerp = Color.Lerp(textStart, textPulse, tick);
            alertText.color = textLerp;

            if (true)
			{

			}
			yield return null;
		}


		alert.enabled = false;

		alert.color = startColor;
		alertText.text = " ";
        yield break;
	}



	public void ResetChaosFactorTimer() { nextChaosFactorTimerSeconds = 0f; }

	private void RemoveAllChaosFactors(bool earlyEnd)
	{
		StopAllCoroutines();
		for (int i = 0; i < currentRunningChaosFactors.Count; i++)
		{
			if (currentRunningChaosFactors[i] == null) continue;
			currentRunningChaosFactors[i].GetComponent<ChaosFactor>().OnEndOfChaosFactor(earlyEnd);
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
		RemoveAllChaosFactors(true);
        alertText.text = " ";
        alert.enabled = false;
		alert.color = startColor;

	}
}
