using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCamera : MonoBehaviour
{
	public void Center()
	{
		transform.position = new Vector3(((GameManager._Instance.LevelBuilder.ColumnCount * GameManager._Instance.LevelBuilder.TileSize) * 0.5f) - 2, transform.position.y, (-GameManager._Instance.LevelBuilder.RowCount * GameManager._Instance.LevelBuilder.TileSize) * 0.5f);

	}
}
