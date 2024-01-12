using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
	Vector2 moveDir, aimDir; //The current movement direction of this player.

	[SerializeField]
	private GameObject pivot;

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
	public void SetFiringDirection(Vector2 dir)
	{
		aimDir = new Vector2(dir.x, dir.y);
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

		var angle = Mathf.Atan2(aimDir.x, aimDir.y) * Mathf.Rad2Deg;
		pivot.transform.rotation = Quaternion.Euler(0, angle, 0);
	}
}
