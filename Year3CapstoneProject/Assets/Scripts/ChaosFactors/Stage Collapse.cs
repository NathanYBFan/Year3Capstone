using System.Collections;
using UnityEngine;

public class StageCollapse : MonoBehaviour, ChaosFactor
{
	[SerializeField]
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

	// Public getter/setters
	public float Timer { get { return timer; } }
	
	// Start is called before the first frame update
	void Start()
	{
		droppedPlatforms = new GameObject[numberOfBlocks];
		StartCoroutine(collapse());
        GameObject.Find("VCam").GetComponent<CameraShake>().ShakeCamera(1, timer);
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
			
			//StartCoroutine(droppedPlatforms[j].GetComponent<Platform>().Up());
			droppedPlatforms[j].GetComponent<Platform>().fakeRespawn();
		}

	}


}
