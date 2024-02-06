using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBody : MonoBehaviour
{
	// Serialize Fields
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private GameObject pivot;
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private GameObject explosion;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private PlayerStats stats;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private PlayerCollision collisionDetection;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private Rigidbody rb;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private AudioSource audioSource;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private GameObject aimUI;


	[SerializeField]
	[Foldout("Stats"), Tooltip("")]
	private int playerIndex = -1; //Which number this player is.

	[SerializeField]
	[Foldout("Stats"), Tooltip("Float value that determines how smoothly the rotation of the aim UI will rotate to adjust to new surface normals.")]
	private float smoothFactor = 1;

	[SerializeField]
	[Foldout("Physics Modifiers"), Tooltip("Flag that allows the movement to switch between default and ice settings." +
		"\nNOTE: Will be read-only in the future. Editable in editor for debugging purposes.")]
	private bool onIce;

	[SerializeField]
	[Foldout("Physics Modifiers"), Range(0, 2), Tooltip("Float value that affects the amount of resistance the player experiences to move in a specific direction." +
		"\nNOTE: Higher values make the resistance against the player greater (and makes it harder to move).")]
	private float iceInertiaMultiplier = 2f;

	private bool hasExploded = false;
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

	public void InitiateSelfDestruct()
	{
		if (stats.canSelfDestruct && !hasExploded)
		{
			GameObject explosionRadius = Instantiate(explosion, transform.position, Quaternion.identity);
			explosionRadius.GetComponent<Explosive>().originalPlayerIndex = playerIndex;
			explosionRadius.GetComponent<Explosive>().playerOwner = stats;
			explosionRadius.GetComponent<Explosive>().StartExpansion(true);
			hasExploded = true;
		}
	}

	public void PerformDash()
	{
		float amount = (2 * stats.MaxEnergy) / 3;
		if (stats.isPowerSaving) amount /= 2;
		if (stats.CurrentEnergy - amount < 0) return;
		stats.UseEnergy(amount);

		Vector3 moveDirection = new Vector3(moveDir.x, 0, moveDir.y);
		if (moveDirection.magnitude > 1)
			moveDirection.Normalize();

		Vector3 velocity = rb.velocity;
		Vector3 newForceDirection = (moveDirection * stats.MovementSpeed);
		velocity.y = 0;
		rb.AddForce(newForceDirection * stats.DashSpeed, ForceMode.Impulse);

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
		AudioManager._Instance.PlayerAudioSourceList.Add(audioSource);
	}


	// Update is called once per frame
	private void FixedUpdate()
	{

		//Aim UI oriented so y axis is parallel to map surface normal
		RaycastHit hit;
		if (Physics.Raycast(aimUI.transform.position, Vector3.down, out hit))
		{
			// Calculate the rotation to match the surface normal
			Quaternion surfaceRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

			// Combine with the pivot's rotation
			Quaternion finalRotation = surfaceRotation * pivot.transform.rotation;

			// Apply the combined rotation to the UI element
			aimUI.transform.rotation = Quaternion.Slerp(aimUI.transform.rotation, finalRotation, smoothFactor); ;

		}
		var angle = Mathf.Atan2(aimDir.x, aimDir.y) * Mathf.Rad2Deg;
		pivot.transform.rotation = Quaternion.Euler(0, angle, 0);


		Vector3 moveDirection = new Vector3(moveDir.x, 0, moveDir.y);
		if (moveDirection.magnitude > 1)
			moveDirection.Normalize();

		if (stats.CurrHealth <= 0 && !stats.IsDead)
		{
			if (stats.canSelfDestruct) InitiateSelfDestruct();
		}
		if (onIce)
		{
			//Apply movement for ice physics.			
			rb.velocity += (moveDirection * stats.MovementSpeed) - collisionDetection.contactNormal * Time.deltaTime;
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, stats.MovementSpeed / 1.5f); //Cannot go above max speed.

			Vector3 velocity = rb.velocity;
			velocity.y = 0;
			rb.velocity -= velocity * iceInertiaMultiplier * Time.deltaTime; //Apply resistance to the player's movement.
		}
		else
		{

			//Default motion.
			Vector3 velocity = rb.velocity;
			Vector3 newForceDirection = (moveDirection * stats.MovementSpeed) - collisionDetection.contactNormal;
			velocity.y = 0;
			rb.AddForce(newForceDirection - velocity, ForceMode.VelocityChange);
		}
	}
}



