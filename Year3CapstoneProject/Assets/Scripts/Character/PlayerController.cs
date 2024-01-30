using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
	private PlayerInput playerInput; // This corresponds to a "Player Input Component" that is attached to the GO with this script on it.
	public PlayerInputActions playerControl; // The player input in general
	private PlayerBody body; // A specific player that will be manipulated by this controller.
	public float temp;

	private float holdDuration = 0.6f;


	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();

		var bodies = FindObjectsOfType<PlayerBody>(); //Getting an array of players...
		var index = playerInput.playerIndex; //This is just fetching the index from the Player Input component.

		body = bodies.FirstOrDefault(m => m.PlayerIndex == index); //The body that this controller corresponds to is the one whose index matches the player input index of this controller.
	}
	 
	public void OnDebugPressed(CallbackContext ctx)
	{
		DebugMenu._Instance.OnDebugPressed();
	}
	public void OnFire(CallbackContext ctx)
	{
		if (!GameManager._Instance.inGame) return;

		if (body == null) return;
		
		body.FireBullet();
	}
	public void OnSelfDestruct(CallbackContext ctx)
	{
		if (!GameManager._Instance.inGame) return;

		float holdTime = (float) ctx.duration;
		if (body == null || holdTime < holdDuration) return;
		
		body.InitiateSelfDestruct();
	}
	public void OnDash(CallbackContext ctx)
	{
		if (!GameManager._Instance.inGame) return;

		if (body == null) return;
		// Add code here
	}
	public void OnAim(CallbackContext ctx)
	{
		if (!GameManager._Instance.inGame) return;
        
		if (body == null) return;

		body.SetFiringDirection(ctx.ReadValue<Vector2>());
	}
	/// <summary>
	/// Event that is called when the player provides movement input.
	/// </summary>
	/// <param name="ctx"></param>
	public void OnMove(CallbackContext ctx)
	{
		if (!GameManager._Instance.inGame) return;
        
		if (body == null) return;

        // If this controller has a body to control, then adjust the direction of which it's going to move.
        body.SetMovementVector(ctx.ReadValue<Vector2>());
	}

	public void OnPause(CallbackContext ctx)
	{
		if (!GameManager._Instance.inGame) return;
        
		if (body == null) return;

		GameManager._Instance.PauseGame();
	}
}
