using NaughtyAttributes;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.VFX;

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
	[Foldout("Dependencies"), Tooltip("")] private ParticleSystem healEffect;
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")] private VisualEffect speedEffect;
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")] private VisualEffect bootEffect;
    [SerializeField]
	[Foldout("Dependencies"), Tooltip("")] private ParticleSystem dmgEffect;

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
	public bool Reset { get { return reset; } set { reset = value; } }
	public bool HasExploded { get { return hasExploded; } set { hasExploded = value; } }
	public bool OnIce { get { return onIce; } set { onIce = value; } }
	public Animation HeadAnim { get { return headAnim; } set { headAnim = value; } }
	public Animation LegAnim { get { return legAnim; } set { legAnim = value; } }
	public int PlayerIndex { get { return playerIndex; } }
	public bool IsRolling { get { return isRolling; } }

	public bool CanMove { get { return canMove; } }

	public bool BootCF { get { return bootCF; } set { bootCF = value; } }
	public GameObject Shield { get { return playerShield; } }

	public GameObject Pivot { get { return pivot; } set { pivot = value; } }
    public GameObject LegPivot { get { return legPivot; } set { legPivot = value; } }


    #endregion Getters & Setters
    #region Private Variables
    Animation headAnim;
	Animation legAnim;
	private bool hasExploded = false;
	private bool isDashing = false, isShooting = false, isRolling = false, canMove = true, reset = false;
	private Vector2 moveDir, aimDir, legDir; //The current movement direction of this player.
	private bool bootCF = false;
	private bool isBooting = false;
	#endregion Private Variables

	private void Start() { AudioManager._Instance.PlayerAudioSourceList.Add(audioSource); }
	private void Update()
	{
		UpdateAnimations();

		// Begin self-destruct (if possible)
		if (stats.CurrentHealth <= 0 && !stats.IsDead)
			if (stats.CanSelfDestruct) StartCoroutine("InitiateSelfDestruct");


		// Scripted roll outcomes
		if (Input.GetKeyDown(KeyCode.Alpha1)) Roll(1);      // Damage boost
		else if (Input.GetKeyDown(KeyCode.Alpha2)) Roll(2); // Shield
		else if (Input.GetKeyDown(KeyCode.Alpha3)) Roll(3); // Speed boost
		else if (Input.GetKeyDown(KeyCode.Alpha4)) Roll(4); // Heal
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

		if (canMove)
		{
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
	}

	private IEnumerator DestroyPlayer()
	{
		while (headAnim.IsPlaying("Death")) yield return null;
		GameManager._Instance.PlayerDied(this.gameObject);
	}

	public void Death()
	{
		moveDir = Vector3.zero;
		headAnim.Play("Death");
		legAnim.Play("Death");
		DeathSound();
		StartCoroutine(DestroyPlayer());
	}

	private void UpdateAnimations()
	{
		if (!GameManager._Instance.InGame) return;
		headAnim = mesh.transform.GetComponentInChildren<Animation>();
		if (headAnim == null) return;

		// Updating head animations accordingly
		if (isDashing)
		{
			headAnim.Play("Dash");
			isDashing = false;
		}
		else if (isShooting && canMove && !isBooting)
		{
			headAnim.Play("Shoot");
			isShooting = false;
		}
		else if (isRolling)
		{
			headAnim.Play("Roll");
			isRolling = false;
		}
		else if (bootCF && isBooting)
		{
			headAnim.Play("Boot");
		}
		else if (canMove && moveDir.magnitude != 0 && !headAnim.IsPlaying("Death") && !headAnim.IsPlaying("Dash") && !headAnim.IsPlaying("Shoot") && !headAnim.IsPlaying("Roll") && !headAnim.IsPlaying("Boot")) headAnim.Play("Walk");
		else if (!headAnim.IsPlaying("Death") && !headAnim.IsPlaying("Dash") && !headAnim.IsPlaying("Shoot") && !headAnim.IsPlaying("Roll") && !headAnim.IsPlaying("Boot") && moveDir.magnitude == 0) headAnim.Play("Idle");


		legAnim = transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetComponent<Animation>();
		if (legAnim == null) return;

		// Updating leg animations accordingly
		if (isDashing)
		{
			legAnim.Play("Dash");
			isDashing = false;
		}
		else if (isShooting && canMove)
		{
			legAnim.Play("Shoot");
			stats.CanShoot = true;
			isShooting = false;
		}
		else if (isRolling)
		{
			legAnim.Play("Roll");
			isRolling = false;
		}
		else if (bootCF && isBooting)
		{
			legAnim.Play("Boot");
			isBooting = false;
		}
		else if (canMove && moveDir.magnitude != 0 && !legAnim.IsPlaying("Death") && !legAnim.IsPlaying("Dash") && !legAnim.IsPlaying("Shoot") && !legAnim.IsPlaying("Roll") && !legAnim.IsPlaying("Boot")) legAnim.Play("Walk");
		else if (!legAnim.IsPlaying("Death") && !legAnim.IsPlaying("Dash") && !legAnim.IsPlaying("Shoot") && !legAnim.IsPlaying("Roll") && !legAnim.IsPlaying("Boot") && moveDir.magnitude == 0) legAnim.Play("Idle");

		if (!headAnim.IsPlaying("Roll"))
		{
			// They are no longer rolling, so we can give the rolling player movement and shooting again
			canMove = true;
			stats.CanShoot = true;
		}


	}

	public void Roll()
	{
		int healing = (int)(stats.MaxHealth - stats.CurrentHealth);
		float amount = stats.MaxEnergy;
		stats.CanShoot = false;

		// Power-Saving Mode
		if (stats.IsPowerSaving) amount = amount * 0.5f; // Must be done somewhere else/should run only once
														 // Has enough energy to roll
		if (stats.CurrentEnergy - amount < 0) return;
		isRolling = true;
		canMove = false;
		rb.velocity = Vector3.zero;

		// Using energy before doing action
		stats.UseEnergy(amount);
		int odds;
		if (stats.CurrentHealth == stats.MaxHealth)
			odds = Random.Range(1, 4);
		else
			odds = Random.Range(1, 5);
		if (odds == 1)
		{
			//Roll for damage boost
			StartCoroutine(DmgBoost());
		}
		else if (odds == 2)
		{

			//Roll for Shield
			StartCoroutine(PlayerShield());
		}
		else if (odds == 3)
		{
			//Roll for movement speed boost
			StartCoroutine(SpeedBoost());
		}
		else if (odds == 4)
		{
			stats.Heal(healing);
			healEffect.Play();
		}
	}

	private void Roll(int input)
	{
		stats.CanShoot = false;
		int healing = (int)(stats.MaxHealth - stats.CurrentHealth);
		if (stats.CurrentEnergy - stats.RollingEnergyConsumption < 0 || stats.IsDead) return;
		isRolling = true;
		canMove = false;
		rb.velocity = Vector3.zero;

		// Using energy before doing action
		stats.UseEnergy(stats.RollingEnergyConsumption);

		switch (input)
		{
			case 1:
				StartCoroutine(DmgBoost());
				break;
			case 2:
				StartCoroutine(PlayerShield());
				break;
			case 3:
				StartCoroutine(SpeedBoost());
				break;
			case 4:
				stats.Heal(healing);
				healEffect.Play();
				break;
		}
	}

	public void FireBullet()
	{
		if (!GameObject.Find("Boot(Clone)")) { bootCF = false; }

		if (Time.time >= stats.NextFireTime && !bootCF)
		{
			GetComponent<PlayerShooting>().FireBullet();
			isShooting = true;
			stats.NextFireTime = Time.time + 1f / stats.FireRate;
		}
		else if (Time.time >= stats.NextFireTime && bootCF == true)
		{
			isBooting = true;
			GameObject.Find("Boot(Clone)").GetComponent<Boot>().Kick(gameObject);
			stats.NextFireTime = Time.time + 1f / stats.FireRate;
			bootEffect.Play();
		}
	}

	public IEnumerator InitiateSelfDestruct()
	{
		if (stats.CanSelfDestruct && !hasExploded)
		{
			hasExploded = true;
			canMove = false;
			GameObject explosionRadius = Instantiate(explosion, transform.position, Quaternion.identity);
			explosionRadius.GetComponent<Explosive>().OriginalPlayerIndex = playerIndex;
			explosionRadius.GetComponent<Explosive>().PlayerOwner = stats;
			explosionRadius.GetComponent<Explosive>().StartExpansion(true);
            GameObject.Find("VCam").GetComponent<CameraShake>().ShakeCamera(1.5f, 0.5f);

            while (explosionRadius != null)
			{
				yield return null;
			}
			stats.StartDeath();
		}
		else yield return null;
	}

	public void DashActionPressed()
	{
		if (stats.CurrentEnergy - stats.DashEnergyConsumption < 0) return;
		isDashing = true;
		dashEffect.Play(true);
		// Using energy before doing action
		stats.UseEnergy(stats.DashEnergyConsumption);

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
			if (stats.IsDead || reset) break;
			float distThisFrame = stats.DashSpeed * Time.deltaTime;
			rb.MovePosition(rb.position + moveDir * distThisFrame);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		reset = false;
		isDashing = false;
		dashEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	private IEnumerator DmgBoost()
	{
		float buffTime = 0f;
		stats.Damage += 1;
		dmgEffect.Play();
		while (buffTime < stats.DamageBoostDuration)
		{
			if (stats.IsDead || reset) break;
			buffTime += Time.deltaTime;
			yield return null;
		}

		stats.Damage -= 1;
		dmgEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	private IEnumerator SpeedBoost()
	{
		float buffTime = 0f;
		stats.MovementSpeed += 3;
		speedEffect.Play();
		while (buffTime < stats.SpeedBoostDuration)
		{
			if (stats.IsDead || reset) break;
			buffTime += Time.deltaTime;
			yield return null;
		}

		reset = false;
		stats.MovementSpeed -= 3;
		speedEffect.Reinit();
		speedEffect.Stop();
	}

	private IEnumerator PlayerShield()
	{
		float currTime = 0f;
		playerShield.SetActive(true);

		while (currTime <= stats.ShieldScaleTime)
		{
			if (stats.IsDead || reset) break;
			// Scale up with scale time
			playerShield.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * stats.ShieldMaxSize, currTime / stats.ShieldScaleTime);
			currTime += Time.deltaTime;
			yield return null;
		}
		currTime = 0;

		while (currTime < stats.ShieldUpDuration)
		{
			if (stats.IsDead || reset) break;
			// Upkeep shield for shield duration time
			currTime += Time.deltaTime;
			yield return null;
		}
		currTime = 0f;

		while (currTime <= stats.ShieldScaleTime)
		{
			if (stats.IsDead || reset) break;
			// Scale down with scale time
			playerShield.transform.localScale = Vector3.Lerp(Vector3.one * stats.ShieldMaxSize, Vector3.zero, currTime / stats.ShieldScaleTime);
			currTime += Time.deltaTime;
			yield return null;
		}
		reset = false;
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

	public void ResetPlayer()
	{
		StopAllCoroutines();
		aimDir = Vector3.zero;
		moveDir = Vector3.zero;
		legDir = Vector3.zero;
		isRolling = false;
		bootEffect.Reinit();
		bootEffect.Stop();
		if (headAnim != null && legAnim != null)
		{
			if (headAnim.clip != null)
			{
				headAnim.Stop();
				legAnim.Stop();
			}
		}
	}
	public void SoftReset()
	{
		reset = true;
	}
}



