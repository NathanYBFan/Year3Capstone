using NaughtyAttributes;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    // Serialize Fields
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject pivot;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private PlayerStats stats;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private Rigidbody rb;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private AudioSource audioSource;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private int playerIndex = -1; //Which number this player is.

    [SerializeField]
    [Foldout("Physics Modifiers"), Tooltip("Flag that allows the movement to switch between default and ice settings." +
        "\nNOTE: Will be read-only in the future. Editable in editor for debugging purposes.")]
    private bool onIce;
    [SerializeField]
    [Foldout("Physics Modifiers"), Range(0,2), Tooltip("Float value that affects the amount of resistance the player experiences to move in a specific direction." +
        "\nNOTE: Higher values make the resistance against the player greater (and makes it harder to move).")]
    private float iceInertiaMultiplier = 0.5f;

    // Getters
    public bool OnIce { get { return onIce; } set { onIce = value; } }
    public int PlayerIndex { get { return playerIndex; } }

    // Private Variables
    private Vector2 moveDir, aimDir; //The current movement direction of this player.

    /// <summary>
    /// Fires a bullet if the player can fire a bullet
    /// </summary>
    public void FireBullet()
    {
        if (Time.time >= stats.nextFireTime)
        {
            GetComponent<PlayerShooting>().FireBullet();
            stats.nextFireTime = Time.time + 1f / stats.FireRate;
        }
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
		GameManager._Instance.Players.Add(this.gameObject);
        AudioManager._Instance.PlayerAudioSourceList.Add(audioSource);
    }

	// Update is called once per frame
	private void Update()
	{
        var angle = Mathf.Atan2(aimDir.x, aimDir.y) * Mathf.Rad2Deg;
        pivot.transform.rotation = Quaternion.Euler(0, angle, 0);


		Vector3 moveDirection = new Vector3(moveDir.x, 0, moveDir.y);
		if (moveDirection.magnitude > 1)
			moveDirection.Normalize();


        if (onIce)
        {
			//Apply movement for ice physics.			
			rb.velocity += (moveDirection * stats.MovementSpeed) * Time.deltaTime;
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, stats.MovementSpeed); //Cannot go above max speed.

			Vector3 velocity = rb.velocity;
			velocity.y = 0;
			rb.velocity -= velocity * iceInertiaMultiplier * Time.deltaTime; //Apply resistance to the player's movement.
		}
        else
        {
            //Default motion.
            Vector3 velocity = rb.velocity;
            velocity.y = 0;
            rb.AddForce((moveDirection * stats.MovementSpeed) - velocity, ForceMode.Acceleration);
        }


    }
}



