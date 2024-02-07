using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
	#region Public Variables
	public PlayerInputActions playerControl; // The player input in general
	public float temp;
	#endregion Public Variables
	#region Private Variables
	private PlayerInput playerInput;	// This corresponds to a "Player Input Component" that is attached to the GO with this script on it.
	private PlayerBody body;			// A specific player that will be manipulated by this controller.
	private float holdDuration = 0.6f;
	InputAction fireAction;				// Input Action for firing bullets (to see if the button is held down).
	#endregion Private Variables

	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();
		var bodies = FindObjectsOfType<PlayerBody>(); // Getting an array of players...
		int index = playerInput.playerIndex; 

		body = bodies.FirstOrDefault(m => m.PlayerIndex == index); // The body that this controller corresponds to is the one whose index matches the player input index of this controller.
		playerInput.uiInputModule = GameManager._Instance.UiInputModule;
		fireAction = playerInput.currentActionMap.FindAction("Fire", true);
	}

	private void Update()
	{
		OnFire();
	}

	public void OnDebugPressed(CallbackContext ctx)	{		DebugMenu._Instance.OnDebugPressed();	}

	public void OnFire()
	{
		if (!GameManager._Instance.inGame) return;
		if (body == null) return;

		if (fireAction.IsPressed()) body.FireBullet();
	}

	public void OnSelfDestruct(CallbackContext ctx)
	{
		if (!GameManager._Instance.inGame) return;
		float holdTime = (float)ctx.duration;
		if (body == null || holdTime < holdDuration) return;

		body.InitiateSelfDestruct();
	}

	public void OnDash(CallbackContext ctx)
	{
		if (!GameManager._Instance.inGame) return;
		if (body == null) return;

		body.PerformDash();
	}

	public void OnAim(CallbackContext ctx)
	{
		if (!GameManager._Instance.inGame) return;
		if (body == null) return;

		body.SetFiringDirection(ctx.ReadValue<Vector2>());
	}

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
