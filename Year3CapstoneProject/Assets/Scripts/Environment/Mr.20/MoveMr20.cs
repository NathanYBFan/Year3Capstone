using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMr20 : MonoBehaviour
{
	#region Serialize Fields
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("List of points to move to")]
	private List<GameObject> pointList;



	[SerializeField]
	[Foldout("Dependencies"), Tooltip("Speed value of platform & Mr. 20")]
	private float speed;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("Rotation speed value of platform & Mr. 20")]
	private float rotSpeed;

	#endregion

	#region Private Variables
	private Vector3 currPos;
	private int targetIndex;
	private bool returning; // whether or not he is on the return trip back to the start
	private Vector3 rotVector;
	float bobSpeed = 2f;
	float bobHeight = 0.5f;
	private float originalY, currY;
	bool chaosFactorActive = false;
	Vector3 targRotation = new Vector3(0, 0, 0);
	private Coroutine actionActive = null;
	#endregion

	// Start is called before the first frame update
	void Start()
	{
		targetIndex = 0;
		returning = false;
		currY = transform.position.y;
		originalY = transform.position.y;
	}

	// Update is called once per frame
	void Update()
	{
		if (!chaosFactorActive)
		{
			if (actionActive == null)
			{
				int randNum = Random.Range(0, 2);
				switch (randNum)
				{
					case 0:
						actionActive = StartCoroutine(CircleArena());
						break;
					case 1:
						actionActive = StartCoroutine(PeerAtPosition(2.5f));
						break;
					default:
						actionActive = StartCoroutine(CircleArena());
						break;
				}
			}

			float newY = currY + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
			transform.position = new Vector3(transform.position.x, newY, transform.position.z);
		}
		else
		{
			float newY = currY + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
			transform.position = new Vector3(transform.position.x, newY, transform.position.z);
		}

	}
	/// <summary>
	/// Behaviour for Mr. 20 to circle the arena (default behaviour)
	/// </summary>
	/// <returns></returns>
	private IEnumerator CircleArena()
	{
		bool completedOneArc = false;
		while (true)
		{
			currPos = gameObject.transform.position;
			float degrees = rotSpeed * Time.deltaTime;
			if (!returning)
			{
				rotVector = new Vector3(0, degrees, 0);
			}
			else
			{
				rotVector = new Vector3(0, -degrees, 0);
			}

			if (completedOneArc && targetIndex > 0) break;
			if (targetIndex == 2)
			{
				if (!returning && gameObject.transform.rotation.eulerAngles.y < 180)
				{
					//rotate clockwise until facing 180 degrees in world space
					gameObject.transform.Rotate(rotVector, Space.World);
				}
				else if (returning && gameObject.transform.rotation.eulerAngles.y > 180)
				{
					gameObject.transform.Rotate(rotVector, Space.World);
				}
			}
			if (targetIndex == 4)
			{
				if (!returning && gameObject.transform.rotation.eulerAngles.y < 270)
				{
					//rotate clockwise until facing 180 degrees in world space
					gameObject.transform.Rotate(rotVector, Space.World);
				}
			}

			if (targetIndex == 0 && gameObject.transform.rotation.eulerAngles.y > 90)
			{
				gameObject.transform.Rotate(rotVector, Space.World);
			}

			gameObject.transform.position = Vector3.MoveTowards(currPos, new Vector3(pointList[targetIndex].transform.position.x, currPos.y, pointList[targetIndex].transform.position.z), speed * Time.deltaTime);


			if (currPos == new Vector3(pointList[targetIndex].transform.position.x, currPos.y, pointList[targetIndex].transform.position.z))
			{
				if (targetIndex + 1 >= pointList.Count)
				{
					completedOneArc = true;
					returning = true;
				}
				if (currPos == new Vector3(pointList[0].transform.position.x, currPos.y, pointList[0].transform.position.z))
				{
					returning = false;
				}

				if (returning && targetIndex - 1 >= 0)
				{
					targetIndex--;
				}
				else if (targetIndex + 1 < pointList.Count)
				{
					targetIndex++;

				}

			}
			yield return null;
		}
		actionActive = null;
		yield break;
	}
	/// <summary>
	/// Behaviour for Mr. 20 to sit idly watching the game at a point
	/// </summary>
	/// <param name="timeSpentPeering">How long Mr. 20 should wait at the spot for.</param>
	/// <returns></returns>
	private IEnumerator PeerAtPosition(float timeSpentPeering)
	{
		int randNum = Random.Range(0, 3);
		randNum *= 2;

		// Get to destination point.
		while (true)
		{
			currPos = gameObject.transform.position;
			float degrees = rotSpeed * Time.deltaTime;

			if (!returning) rotVector = new Vector3(0, degrees, 0);
			else rotVector = new Vector3(0, -degrees, 0);


			if (targetIndex == 2)
			{
				if (!returning && gameObject.transform.rotation.eulerAngles.y < 180)
				{
					//rotate clockwise until facing 180 degrees in world space
					gameObject.transform.Rotate(rotVector, Space.World);
				}
				else if (returning && gameObject.transform.rotation.eulerAngles.y > 180)
				{
					gameObject.transform.Rotate(rotVector, Space.World);
				}
			}
			if (targetIndex == 4)
			{
				if (!returning && gameObject.transform.rotation.eulerAngles.y < 270)
				{
					//rotate clockwise until facing 180 degrees in world space
					gameObject.transform.Rotate(rotVector, Space.World);
				}
			}

			if (targetIndex == 0 && gameObject.transform.rotation.eulerAngles.y > 90)
			{
				gameObject.transform.Rotate(rotVector, Space.World);
			}

			gameObject.transform.position = Vector3.MoveTowards(currPos, new Vector3(pointList[targetIndex].transform.position.x, currPos.y, pointList[targetIndex].transform.position.z), speed * Time.deltaTime);


			if (currPos == new Vector3(pointList[targetIndex].transform.position.x, currPos.y, pointList[targetIndex].transform.position.z))
			{
				if (targetIndex == randNum) break;
				if (targetIndex + 1 >= pointList.Count)
				{
					returning = true;
				}
				if (currPos == new Vector3(pointList[0].transform.position.x, currPos.y, pointList[0].transform.position.z))
				{
					returning = false;
				}

				if (returning && targetIndex - 1 >= 0)
				{
					targetIndex--;
				}
				else if (targetIndex + 1 < pointList.Count)
				{
					targetIndex++;

				}

			}
			yield return null;
		}
		float currTime = 0;
		while (currTime < timeSpentPeering)
		{
			currTime += Time.deltaTime;
			yield return null;
		}
		returning = true;
		while (true)
		{
			currPos = gameObject.transform.position;
			float degrees = rotSpeed * Time.deltaTime;

			if (!returning) rotVector = new Vector3(0, degrees, 0);
			else rotVector = new Vector3(0, -degrees, 0);


			if (targetIndex == 2)
			{
				if (!returning && gameObject.transform.rotation.eulerAngles.y < 180)
				{
					//rotate clockwise until facing 180 degrees in world space
					gameObject.transform.Rotate(rotVector, Space.World);
				}
				else if (returning && gameObject.transform.rotation.eulerAngles.y > 180)
				{
					gameObject.transform.Rotate(rotVector, Space.World);
				}
			}
			if (targetIndex == 4)
			{
				if (!returning && gameObject.transform.rotation.eulerAngles.y < 270)
				{
					//rotate clockwise until facing 180 degrees in world space
					gameObject.transform.Rotate(rotVector, Space.World);
				}
			}

			if (targetIndex == 0 && gameObject.transform.rotation.eulerAngles.y > 90)
			{
				gameObject.transform.Rotate(rotVector, Space.World);
			}

			gameObject.transform.position = Vector3.MoveTowards(currPos, new Vector3(pointList[targetIndex].transform.position.x, currPos.y, pointList[targetIndex].transform.position.z), speed * Time.deltaTime);


			if (currPos == new Vector3(pointList[targetIndex].transform.position.x, currPos.y, pointList[targetIndex].transform.position.z))
			{
				if (targetIndex == 0) break;
				if (targetIndex + 1 >= pointList.Count) returning = true;
				if (currPos == new Vector3(pointList[0].transform.position.x, currPos.y, pointList[0].transform.position.z)) returning = false;

				if (returning && targetIndex - 1 >= 0) targetIndex--;
				else if (targetIndex + 1 < pointList.Count) targetIndex++;
			}
			yield return null;
		}

		actionActive = null;
		yield break;
	}

	/// <summary>
	/// Behaviour for Mr. 20 when a Chaos Factor gets called
	/// </summary>
	/// <returns></returns>
	private IEnumerator CallChaosFactor()
	{
		Vector3 center = new Vector3((GameManager._Instance.LevelBuilder.ColumnCount * GameManager._Instance.LevelBuilder.TileSize * 0.5f) - 2, transform.position.y, (GameManager._Instance.LevelBuilder.RowCount * GameManager._Instance.LevelBuilder.TileSize * 0.5f) - 6f);


		while (Mathf.Abs(transform.position.x - center.x) > 0.01f && Mathf.Abs(transform.position.z - center.z) > 0.01f)
		{
			Vector3 currPos = transform.position;
			targRotation.y = 180;
			targRotation.x = 0;
			Quaternion targetRotation = Quaternion.Euler(targRotation);
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
			currY = Mathf.Lerp(currY, originalY + 6, Time.deltaTime);
			gameObject.transform.position = Vector3.MoveTowards(currPos, center, speed * 7.5f * Time.deltaTime);
			yield return null;
		}
		GetComponentInChildren<Animation>().Play("Take 001");
		float currTime = 0;
		while (currTime < 3f)
		{
			currTime += Time.deltaTime;
			yield return null;
		}
		while (Mathf.Abs(transform.position.x - pointList[targetIndex].transform.position.x) > 0.01f || Mathf.Abs(transform.position.z - pointList[targetIndex].transform.position.z) > 0.01f)
		{
			Vector3 currPos = transform.position;
			switch (targetIndex)
			{
				case 0:
					targRotation.y = 90;
					break;
				case 1:
					if (returning)
						targRotation.y = 180;
					else
						targRotation.y = 90;
					break;
				case 2:
					targRotation.y = 180;
					break;
				case 3:
					if (returning)
						targRotation.y = 270;
					else
						targRotation.y = 180;
					break;
				case 4:
					targRotation.y = 270;
					break;

			}
			Quaternion targetRotation = Quaternion.Euler(targRotation);
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
			currY = Mathf.Lerp(currY, originalY, Time.deltaTime);
			gameObject.transform.position = Vector3.MoveTowards(currPos, pointList[targetIndex].transform.position, speed * 7.5f * Time.deltaTime);
			yield return null;
		}
		targRotation.x = 10;
		chaosFactorActive = false;
		actionActive = null;
		yield break;
	}
	public void InitiateChaosFactor()
	{
		StopAllCoroutines();
		chaosFactorActive = true;
		actionActive = StartCoroutine(CallChaosFactor());
	}


}
