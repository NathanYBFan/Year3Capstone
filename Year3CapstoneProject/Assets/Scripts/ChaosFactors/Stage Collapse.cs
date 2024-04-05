using System.Collections;
using System.Data;
using UnityEngine;

public class StageCollapse : MonoBehaviour, ChaosFactor
{

    [SerializeField]
    private float percentofBlocks;

    private int numberOfBlocks;

	[SerializeField]
	private float dropInterval;

	[SerializeField]
	private float endDelay;

	[SerializeField]
	private float timer;

	private int randomNum;
	private GameObject[] droppedPlatforms;

	float shakeDelay = 0.025f;
	float shakeAmount = 0.05f;
	float shakeDuration = 0.01f;

	private GeneratesRumble rumble;

	// Public getter/setters
	public float Timer { get { return timer; } }
	
	// Start is called before the first frame update
	void Start()
	{
		numberOfBlocks = (int)(GameManager._Instance.Platforms.Count * percentofBlocks);

        rumble = GetComponent<GeneratesRumble>();
		droppedPlatforms = new GameObject[numberOfBlocks];
		StartCoroutine(collapse());
        GameObject.Find("VCam").GetComponent<CameraShake>().ShakeCamera(1, timer);
		for (int i = 0; i < 4; i++)
			StartCoroutine(GameManager._Instance.CreateRumble(rumble.RumbleDuration, rumble.LeftIntensity, rumble.RightIntensity, i, false));
	}
	public IEnumerator collapse()
	{
		for (int i = 0; i < numberOfBlocks; i++)
		{
			randomNum = Random.Range(0, GameManager._Instance.Platforms.Count);

			GameObject dropping = GameManager._Instance.Platforms[randomNum];
			droppedPlatforms[i] = dropping;

			float currTime = 0;
			Vector3 randomPos;
			Vector3 startingPos = dropping.transform.position;
			while (currTime <= shakeDuration)
			{
				currTime += Time.deltaTime;
				Vector2 xzRandomPos = Random.insideUnitCircle;
				randomPos = startingPos + (new Vector3(xzRandomPos.x, 0, xzRandomPos.y) * shakeAmount);
				dropping.transform.position = randomPos;
				yield return new WaitForSeconds(shakeDelay);
			}
			dropping.transform.position = startingPos;


			dropping.GetComponent<Platform>().collapse();

			yield return new WaitForSeconds(dropInterval);
		}

		float elapsedTime = 0;
		while (elapsedTime < endDelay)
		{
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		c();
		
	}


	private void c()
	{
		for (int j = 0; j < numberOfBlocks; j++)
		{
			
			droppedPlatforms[j].GetComponent<Platform>().fakeRespawn();
		}

	}

	public void OnEndOfChaosFactor(bool earlyEnd)
	{
		GameObject.Find("VCam").GetComponent<CameraShake>().ShakeCamera(0, 1);
		for (int i = 0; i < 4; i++)
			StartCoroutine(GameManager._Instance.StopRumble(i));
		Destroy(gameObject);
	}
}
