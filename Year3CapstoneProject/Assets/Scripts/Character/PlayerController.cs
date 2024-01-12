using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using  UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
	private PlayerInput playerInput; //This corresponds to a "Player Input Component" that is attached to the GO with this script on it.
	public PlayerInputActions playerControl; //The player input in general
	private PlayerBody body; //A specific player that will be manipulated by this controller.

	private void Awake()
	{
		playerControl = new PlayerInputActions();
		playerInput = GetComponent<PlayerInput>();

		var bodies = FindObjectsOfType<PlayerBody>(); //Getting an array of players...
		var index = playerInput.playerIndex; //This is just fetching the index from the Player Input component.

		body = bodies.FirstOrDefault(m => m.PlayerIndex == index); //The body that this controller corresponds to is the one whose index matches the player input index of this controller.
	}

	public void OnFire(CallbackContext ctx)
	{
		if (body != null)
		{

		}
	}
	public void OnDash(CallbackContext ctx)
	{
		if (body != null)
		{

		}
	}
	public void OnAim(CallbackContext ctx)
	{
		if (body != null)
		{
			body.SetFiringDirection(ctx.ReadValue<Vector2>());
		}
	}
	/// <summary>
	/// Event that is called when the player provides movement input.
	/// </summary>
	/// <param name="ctx"></param>
	public void OnMove(CallbackContext ctx)
	{	
		if (body != null) //If this controller has a body to control, then adjust the direction of which it's going to move.
			body.SetMovementVector(ctx.ReadValue<Vector2>());
	}
	
}
