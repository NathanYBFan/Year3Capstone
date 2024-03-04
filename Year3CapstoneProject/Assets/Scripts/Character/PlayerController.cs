using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
	#region Public Variables
	public PlayerInputActions playerControl; // The player input in general
	public float temp;
	#endregion Public Variables

	#region Private Variables
	private PlayerInput playerInput;    // This corresponds to a "Player Input Component" that is attached to the GO with this script on it.
	private PlayerBody body;            // A specific player that will be manipulated by this controller.
	private float holdDuration = 0.6f;
	InputAction fireAction;             // Input Action for firing bullets (to see if the button is held down).
	#endregion Private Variables

	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();
		var bodies = FindObjectsOfType<PlayerBody>(); // Getting an array of players...
		int index = playerInput.playerIndex;

		body = bodies.FirstOrDefault(m => m.PlayerIndex == index); // The body that this controller corresponds to is the one whose index matches the player input index of this controller.
		fireAction = playerInput.currentActionMap.FindAction("Fire", true);

		MenuInputManager._Instance.PlayerInputs.Add(this.gameObject);
		MenuInputManager._Instance.ControllerRejoinEvent(playerInput.playerIndex);
    }

	private void Update()
	{
        OnFire();
	}

	public void OnDebugPressed(CallbackContext ctx) { DebugMenu._Instance.OnDebugPressed(); }

	public void OnRoll(CallbackContext ctx)
	{
		if (!ctx.performed) return;
		if (!GameManager._Instance.InGame) return;
		if (body == null) return;
        if (body.GetComponent<PlayerStats>().IsDead) return;
		if (body.IsRolling) return;

        body.Roll();
	}
	public void OnFire()
	{
		if (!GameManager._Instance.InGame) return;
		if (body == null) return;
        if (body.GetComponent<PlayerStats>().IsDead) return;

        if (fireAction.IsPressed()) body.FireBullet();
	}

	public void OnSelfDestruct(CallbackContext ctx)
	{
		if (!GameManager._Instance.InGame) return;
		float holdTime = (float)ctx.duration;
		if (body == null || holdTime < holdDuration) return;
        if (body.GetComponent<PlayerStats>().IsDead) return;

        body.StartCoroutine("InitiateSelfDestruct");
    }

	public void OnDash(CallbackContext ctx)
	{
        if (!ctx.performed) return;
        if (!GameManager._Instance.InGame) return;
		if (body == null) return;
        if (body.GetComponent<PlayerStats>().IsDead) return;

        body.DashActionPressed();
	}

	public void OnAim(CallbackContext ctx)
	{
		if (!GameManager._Instance.InGame) return;
		if (body == null) return;
        if (body.GetComponent<PlayerStats>().IsDead) return;

        body.SetFiringDirection(ctx.ReadValue<Vector2>());
	}

	public void OnMove(CallbackContext ctx)
	{
		if (!GameManager._Instance.InGame) return;

		if (body == null) return;
		if (body.GetComponent<PlayerStats>().IsDead) return;

		// If this controller has a body to control, then adjust the direction of which it's going to move.
		body.SetMovementVector(ctx.ReadValue<Vector2>());
	}

	public void OnPause(CallbackContext ctx)
	{
		if (!GameManager._Instance.InGame) return;
		if (body == null) return;
        if (body.GetComponent<PlayerStats>().IsDead) return;

        GameManager._Instance.PauseGame(true);
    }
    public void OnSubmitClicked(CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (!MenuInputManager._Instance.InCharacterSelect) return;
        // In character select
        MenuInputManager._Instance.CharacterSelectMenu.CharacterSelectMenus[playerInput.playerIndex].GetComponent<MultiplayerEventSystem>().playerRoot.GetComponent<CharacterSelectUnit>().ConfirmSelections();
    }

    public void OnCancelClicked(CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (!MenuInputManager._Instance.InCharacterSelect) return;
        // In character select
        MenuInputManager._Instance.CharacterSelectMenu.CharacterSelectMenus[playerInput.playerIndex].GetComponent<MultiplayerEventSystem>().playerRoot.GetComponent<CharacterSelectUnit>().CancelSelection();
    }

    public void OnNavigate(CallbackContext ctx)
    {
        if (!ctx.performed) return;

        Vector2 input = ctx.ReadValue<Vector2>().normalized;

        if (!MenuInputManager._Instance.InCharacterSelect) return;

        // In character select
        if (input.x > 0)
			MenuInputManager._Instance.CharacterSelectMenu.CharacterSelectMenus[playerInput.playerIndex].GetComponent<CharacterSelectUnit>().RightPressed();
        else if (input.x < 0)
            MenuInputManager._Instance.CharacterSelectMenu.CharacterSelectMenus[playerInput.playerIndex].GetComponent<CharacterSelectUnit>().LeftPressed();
        // No up or down press
    }
}
