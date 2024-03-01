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
	[Foldout("Dependencies"), Tooltip("")] private GameObject legPivot;
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
	[Foldout("Dependencies"), Tooltip("")] private GameObject playerShield;
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")] private ParticleSystem dashEffect;

	[SerializeField]
	[Foldout("Stats"), Tooltip("")] private int playerIndex = -1; //Which number this player is.

	[SerializeField]
	[Foldout("Stats"), Tooltip("How fast the UI adjusts to a new angle.")]
	private float smoothFactor = 1;

	[SerializeField, ReadOnly]
	[Foldout("Physics Modifiers"), Tooltip("Flag that allows the movement to switch between default and ice settings.")]
	private bool onIce;

	[SerializeField]
	[Foldout("Physics Modifiers"), Range(0, 100), Tooltip("Float value that affects the amount of resistance the player experiences to move in a specific direction." +
		"\nNOTE: Higher values make the resistance against the player greater (and makes it harder to move).")]
	private float iceInertiaMultiplier = 50f;
	#endregion Serialize Fields
	#region Getters & Setters
	public bool OnIce { get { return onIce; } set { onIce = value; } }
	public int PlayerIndex { get { return playerIndex; } }
	public bool IsRolling { get { return isRolling; } }
	#endregion Getters & Setters
	#region Private Variables
	Animation headAnim;
	Animation legAnim;
	private bool hasExploded = false;
	private bool isDashing = false, isShooting = false, isRolling = false;
	private Vector2 moveDir, aimDir, legDir; //The current movement direction of this player.
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
			if (stats.CanSelfDestruct) InitiateSelfDestruct();
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

		// Rotation of the player.
		float bodyAngle = Mathf.Atan2(legDir.x, legDir.y) * Mathf.Rad2Deg;
		legPivot.transform.rotation = Quaternion.Euler(0, bodyAngle, 0);

		// Movement of the player.
		Vector3 moveDirection = new Vector3(moveDir.x, 0, moveDir.y);
		if (moveDirection.magnitude > 1)
			moveDirection.Normalize();

		Vector3 velocity = rb.velocity;
		Vector3 newForceDirection = (moveDirection * stats.MovementSpeed) - collisionDetection.ContactNormal;
		velocity.y = 0;

		if (onIce)
		{
			velocity.y = 0;
			Vector3 iceVelocity = Vector3.Lerp(velocity, (newForceDirection - velocity) * 1.5f, iceInertiaMultiplier * Time.deltaTime);
			rb.AddForce(iceVelocity, ForceMode.Acceleration);
		}
		else
		{
			rb.AddForce(newForceDirection - velocity, ForceMode.VelocityChange);
		}
	}

	private IEnumerator DestroyPlayer()
	{
		while (headAnim.IsPlaying("Death"))
		{
			yield return null;
		}
		//Destroy(gameObject);
        GameManager._Instance.PlayerDied(this.gameObject);
    }

    public void Death()
	{
		headAnim.Play("Death");
		legAnim.Play("Death");
		DeathSound();
		StartCoroutine("DestroyPlayer");
	}
	private void UpdateAnimations()
	{
		if (!GameManager._Instance.InGame) return;
		headAnim = mesh.transform.GetComponentInChildren<Animation>();
		if (headAnim == null) return;

		if (isDashing)
		{
			headAnim.Play("Dash");
			isDashing = false;
		}
		else if (isShooting)
		{
			headAnim.Play("Shoot");
			isShooting = false;
		}
		else if (isRolling)
		{
			headAnim.Play("Roll");
			isRolling = false;
		}
		else if (moveDir.magnitude != 0 && !headAnim.IsPlaying("Death") && !headAnim.IsPlaying("Dash") && !headAnim.IsPlaying("Shoot") && !headAnim.IsPlaying("Roll")) headAnim.Play("Walk");
		else if (!headAnim.IsPlaying("Death") && !headAnim.IsPlaying("Dash") && !headAnim.IsPlaying("Shoot") && !headAnim.IsPlaying("Roll") && !headAnim.IsPlaying("Walk")) headAnim.Play("Idle");


		legAnim = transform.GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetComponent<Animation>();
		if (legAnim == null) return;

		if (isDashing)
		{
			legAnim.Play("Dash");
			isDashing = false;
		}
		else if (isShooting)
		{
			legAnim.Play("Shoot");
			isShooting = false;
		}
		else if (isRolling)
		{
			legAnim.Play("Roll");
			isRolling = false;
		}
		else if (moveDir.magnitude != 0 && !legAnim.IsPlaying("Death") && !legAnim.IsPlaying("Dash") && !legAnim.IsPlaying("Shoot") && !legAnim.IsPlaying("Roll")) legAnim.Play("Walk");
		else if (!legAnim.IsPlaying("Death") && !legAnim.IsPlaying("Dash") && !legAnim.IsPlaying("Shoot") && !legAnim.IsPlaying("Roll") && !legAnim.IsPlaying("Walk")) legAnim.Play("Idle");
	}

	public void Roll()
	{
		int healing = 1;
		float amount = stats.MaxEnergy;
		// Power saving
        if (stats.IsPowerSaving) amount = amount * 0.5f; // Must be done somewhere else/should run only once
		// Has enough energy to roll
        if (stats.CurrentEnergy - amount < 0) return;
        isRolling = true;

        // Using energy before doing action
        stats.UseEnergy(amount);
        

        int odds = Random.Range(1, 5);
		if (odds == 1) 
		{
			//Roll for damage boost
			StartCoroutine(DmgBoost());
			Debug.Log("KYS tehe :3");
		}
		else if (odds == 2)
		{
			//Roll for heal, needs animation still
			stats.Heal(healing);
			Debug.Log("Meow");
		}
		else if (odds == 3)
		{
			//Roll for movement speed boost
			StartCoroutine(SpeedBoost());
			Debug.Log("Benguin");
		}
		else if (odds == 4)
		{
			//Roll for Shield
			StartCoroutine(PlayerShield());
			Debug.Log("Huh");
		}
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
		if (stats.CanSelfDestruct && !hasExploded)
		{
			GameObject explosionRadius = Instantiate(explosion, transform.position, Quaternion.identity);
			explosionRadius.GetComponent<Explosive>().OriginalPlayerIndex = playerIndex;
			explosionRadius.GetComponent<Explosive>().PlayerOwner = stats;
			explosionRadius.GetComponent<Explosive>().StartExpansion(true);
			hasExploded = true;
		}
	}

	public void DashActionPressed()
	{
        float amount = stats.MaxEnergy / 3.0f;
        // Power saving
        if (stats.IsPowerSaving) amount = amount * 0.5f; // Must be done somewhere else/should run only once
                                                         // Has enough energy to dash
        if (stats.CurrentEnergy - amount < 0) return;

		isDashing = true;
		dashEffect.Play(true);
		// Using energy before doing action
		stats.UseEnergy(amount);
        

        Vector3 moveDirection = new Vector3(moveDir.x, 0, moveDir.y);
		if (moveDirection.magnitude > 1)
			moveDirection.Normalize();

		StartCoroutine(PerformDash(moveDirection));

	}
	private IEnumerator PerformDash(Vector3 moveDir)
	{
		float elapsedTime = 0f;
		while (elapsedTime < stats.DashDuration)
		{
			float distThisFrame = stats.DashSpeed * Time.deltaTime;

			rb.MovePosition(rb.position + moveDir * distThisFrame);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		isDashing = false;
		dashEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	private IEnumerator DmgBoost()
	{
		//needs anim
        stats.Damage = stats.Damage + 1;
        yield return new WaitForSeconds(3f);
		stats.Damage = stats.Damage - 1;
	}

	private IEnumerator SpeedBoost()
	{
		//needs to play animation still
		
        stats.MovementSpeed = stats.MovementSpeed + 2;
		yield return new WaitForSeconds(3f);
		stats.MovementSpeed = stats.MovementSpeed - 2;
    }

	private IEnumerator PlayerShield()
	{
		//im hoping this fades in and out the shield, duration of shield is 5s currently.
		float shieldTime = 0f;
        playerShield.SetActive(true);
        Color c = playerShield.GetComponent<Renderer>().material.color;
        for (float alpha = 0f; alpha >= 1; alpha += 0.1f)
        {
            c.a = alpha;
            playerShield.GetComponent<Renderer>().material.color = c;
            yield return new WaitForSeconds(.1f);
        }
        while (shieldTime < 5f)
		{		
			shieldTime += Time.deltaTime;
			yield return null;
        }
        Color s = playerShield.GetComponent<Renderer>().material.color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            s.a = alpha;
            playerShield.GetComponent<Renderer>().material.color = s;
            yield return new WaitForSeconds(.1f);
        }
        playerShield.SetActive(false);
    }
	//Plays the player death sound
    private void DeathSound()
    {
        float randPitch = Random.Range(0.8f, 1.5f);
        audioSource.pitch = randPitch;
        AudioManager._Instance.PlaySoundFX(AudioManager._Instance.PlayerAudioList[2], audioSource);
    }
    public void SetMovementVector(Vector2 dir) { moveDir = dir; if (dir.x != 0 && dir.y != 0) legDir = dir; }

	public void SetFiringDirection(Vector2 dir) { if (dir.x != 0 && dir.y != 0) aimDir = dir; }
}



