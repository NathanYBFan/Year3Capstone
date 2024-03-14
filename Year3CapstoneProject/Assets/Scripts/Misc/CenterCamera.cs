using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCamera : MonoBehaviour
{
	public void Center()
	{
		switch (GameManager._Instance.LevelBuilder.CurrentLevelName)
		{
			case "Scrapbox":
				transform.position = new Vector3(((17 * 4) * 0.5f) - 2, transform.position.y, (-11 * 4) * 0.5f);
				break;
			case "OopsAllConveyors":
				transform.position = new Vector3(((15 * 4) * 0.5f) - 2, transform.position.y, (-12 * 4) * 0.5f);
				break;
			case "Toasty":
				transform.position = new Vector3(((15 * 4) * 0.5f) - 2, transform.position.y, (-10 * 4) * 0.5f);
				break;
		}
	}
}
