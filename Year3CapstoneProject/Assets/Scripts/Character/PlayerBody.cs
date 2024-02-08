using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBody : MonoBehaviour
{
	#region Serialize Fields
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")] private GameObject pivot;
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")] private GameObject mesh;
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")] private GameObject explosion;
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")] private PlayerStats stats;
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")] private PlayerCollision collisionDetection;
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")] private Rigidbody rb;
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")] private AudioSource audioSource;
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")] private GameObject aimUI;

	[SerializeField]
	[Foldout("Stats"), Tooltip("")] private int playerIndex = -1; //Which number this player is.

	[SerializeField]
	[Foldout("Stats"), Tooltip("How fast the UI adjusts to a new angle.")]
	private float smoothFactor = 1;

	[SerializeField, ReadOnly]
	[Foldout("Physics Modifiers"), Tooltip("Flag that allows the movement to switch between default and ice settings.")]
	private bool onIce;

	[SerializeField]
	[Foldout("Physics Modifiers"), Range(0, 2), Tooltip("Float value that affects the amount of resistance the player experiences to move in a specific direction." +
		"\nNOTE: Higher values make the resistance against the player greater (and makes it harder to move).")]
	private float iceInertiaMultiplier = 2f;
	#endregion Serialize Fields
	#region Getters & Setters
	public bool OnIce { get { return onIce; } set { onIce = value; } }
	public int PlayerIndex { get { return playerIndex; } }
	#endregion Getters & Setters
	#region Private Variables
	Animation anim;
	private bool hasExploded = false;
	private bool isDashing = false, isShooting = false, isDead = false, isRolling = false;
	private Vector2 moveDir, aimDir; //The current movement direction of this player.
	#endregion Private Variables

	private void Start()
	{
		AudioManager._Instance.PlayerAudioSourceList.Add(audioSource);
	}

	private void Update()
	{
		UpdateAnimations();
		// Begin self-destruct (if possible)
		if (stats.CurrentHealth <= 0 && !stats.IsDead)
		{
			if (stats.canSelfDestruct) InitiateSelfDestruct();
		}
	}

	private void FixedUpdate()
	{

		// Aim UI oriented so y-axis is parallel to the surface normal of the map.
		RaycastHit hit;
		if (Physics.Raycast(aimUI.transform.position, Vector3.down, out hit))
		{
			Quaternion surfaceRotation = Quaternion.FromToRotation(Vector3.up, hit.normal); // Calculate the rotation to match the surface normal
			Quaternion finalRotation = surfaceRotation * pivot.transform.rotation;          // Combine with the pivot's rotation

			aimUI.transform.rotation = Quaternion.Slerp(aimUI.transform.rotation, finalRotation, smoothFactor); ;
		}

		// Rotation of the player.
		float angle = Mathf.Atan2(aimDir.x, aimDir.y) * Mathf.Rad2Deg;
		pivot.transform.rotation = Quaternion.Euler(0, angle, 0);

		// Movement of the player.
		Vector3 moveDirection = new Vector3(moveDir.x, 0, moveDir.y);
		if (moveDirection.magnitude > 1)
			moveDirection.Normalize();

		if (onIce)
		{
			// Ice Physics Movement.		
			rb.velocity += (moveDirection * stats.MovementSpeed) - collisionDetection.ContactNormal * Time.deltaTime;
			rb.velocity = Vector3.ClampMagnitude(rb.velocity, stats.MovementSpeed / 1.5f); // Cannot go above max speed.

			Vector3 velocity = rb.velocity;
			velocity.y = 0;
			rb.velocity -= velocity * iceInertiaMultiplier * Time.deltaTime; // Apply resistance to the player's movement.
		}
		else
		{
			// Default Movement.
			Vector3 velocity = rb.velocity;
			Vector3 newForceDirection = (moveDirection * stats.MovementSpeed) - collisionDetection.ContactNormal;
			velocity.y = 0;
			rb.AddForce(newForceDirection - velocity, ForceMode.VelocityChange);
		}
	}

	private IEnumerator DestroyPlayer()
	{
		while (anim.IsPlaying("Death"))
		{
			yield return null;
		}
		Destroy(gameObject);

	}
	public void Death()
	{
		isDead = true;
		anim.Play("Death");
		StartCoroutine("DestroyPlayer");

	}
	private void UpdateAnimations()
	{
		if (!GameManager._Instance.InGame) return;
		anim = mesh.transform.GetChild(0).GetChild(0).GetComponent<Animation>();
		if (anim == null) return;

		if (isDashing)
		{
			anim.Play("Dash");
			isDashing = false;
		}
		else if (isShooting)
		{
			anim.Play("Shoot");
			isShooting = false;
		}
		else if (isRolling)
		{
			anim.Play("Roll");
			isRolling = false;
		}
		else if (moveDir.magnitude != 0 && !anim.IsPlaying("Death") && !anim.IsPlaying("Dash") && !anim.IsPlaying("Shoot") && !anim.IsPlaying("Roll")) anim.Play("Walk");
		else if (!anim.IsPlaying("Death") && !anim.IsPlaying("Dash") && !anim.IsPlaying("Shoot") && !anim.IsPlaying("Roll") && !anim.IsPlaying("Walk")) anim.Play("Idle");


	}

	public void Roll()
	{
		isRolling = true;
	}
	public void FireBullet()
	{
		if (Time.time >= stats.NextFireTime)
		{
			GetComponent<PlayerShooting>().FireBullet();
			isShooting = true;
			stats.NextFireTime = Time.time + 1f / stats.FireRate;
		}
	}

	public void InitiateSelfDestruct()
	{
		if (stats.canSelfDestruct && !hasExploded)
		{
			GameObject explosionRadius = Instantiate(explosion, transform.position, Quaternion.identity);
			explosionRadius.GetComponent<Explosive>().OriginalPlayerIndex = playerIndex;
			explosionRadius.GetComponent<Explosive>().PlayerOwner = stats;
			explosionRadius.GetComponent<Explosive>().StartExpansion(true);
			hasExploded = true;
		}
	}

	public void PerformDash()
	{
		isDashing = true;
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

	public void SetMovementVector(Vector2 dir) { moveDir = dir; }

	public void SetFiringDirection(Vector2 dir) { if (dir.x != 0 && dir.y != 0) aimDir = dir; }
}



