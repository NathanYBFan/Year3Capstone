using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
	Vector2 moveDir, aimDir; //The current movement direction of this player.

	[SerializeField]
	private GameObject pivot;


	[SerializeField]
	public ChaosFactorManager CFManager;


	private Rigidbody rb;

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
		if (dir.x != 0 && dir.y != 0)
			aimDir = dir;
	}
	private void Start()
	{
		stats = GetComponent<PlayerStats>();
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	private void Update()
	{
		Vector3 moveDirection = new Vector3(moveDir.x, 0, moveDir.y);
		if (moveDirection.magnitude > 1)
		{
			moveDirection.Normalize();
		}

		rb.AddForce((moveDirection * stats.MovementSpeed) - rb.velocity, ForceMode.Acceleration);
		var angle = Mathf.Atan2(aimDir.x, aimDir.y) * Mathf.Rad2Deg;
		pivot.transform.rotation = Quaternion.Euler(0, angle, 0);
	}


	public void choasFactorTest(int toTest)
	{
		Debug.Log("Running test function in player body");
		CFManager.StartChaosFactorTest(toTest - 1);
		Debug.Log("completed test function in player body");
	}




}



