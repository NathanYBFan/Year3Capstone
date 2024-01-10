using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
	Vector2 moveDir; //The current movement direction of this player.
	PlayerStats stats;

	[SerializeField]
	private int playerIndex = -1; //Which number this player is.
	public int PlayerIndex
	{
		get { return playerIndex; }
	}    

	/// <summary>
	/// Sets the directional vector for movement.
	/// </summary>
	/// <param name="dir">The direction vector.</param>
	public void SetMovementVector(Vector2 dir)
	{
		moveDir = dir;
	}

	private void Start()
	{
		stats = GetComponent<PlayerStats>();
	}

	// Update is called once per frame
	private void Update()
	{
		Vector3 moveDirection = new Vector3(moveDir.x, 0, moveDir.y);
		if (moveDirection.magnitude > 1)
		{
			moveDirection.Normalize();
		}

		this.gameObject.GetComponent<Rigidbody>().velocity = moveDirection * stats.MovementSpeed;

	}
}
