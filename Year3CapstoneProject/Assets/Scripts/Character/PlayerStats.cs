using System;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering;
using UnityEditor.ShaderGraph.Internal;

public class PlayerStats : MonoBehaviour
{
	#region Serialize Fields
	[SerializeField, ReadOnly]
	[Foldout("Dependencies"), Tooltip("Character Stat scriptable object of stats to assign")]
	private CharacterStatsSO characterStat;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private Transform playerMeshGO;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private Transform playerLegGO;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("The particle system prefabs for debuff effects")]
	private Material playerGlowMaterial;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("The particle system prefabs for debuff effects")]
	private Material playerAimUIMaterial;
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("The particle system prefabs for debuff effects")]
	private Bars playerHUD;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("The intensity value to apply globally to all player emissions.")]
	private float playerEmissionIntensity = 3.42f;

	[Header("Effects")]
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("The particle system prefabs for debuff effects")]
	private ParticleSystem burning;

	[Header("Character Stats")]
	[SerializeField]
	[Foldout("Player Stats"), Tooltip("Time (in seconds) that a player cannot take damage from certain damage types.")]
	private float invincibilityTime = 0.1f;
	[SerializeField]
	[Foldout("Player Stats"), Tooltip("Player max health")]
	private float maxHealth = 100;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("Player current health")]
	private float currHealth = 100;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("Player max move speed")]
	private float movementSpeed = 10;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("Player max firerate")]
	private float fireRate = 0;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("")]
	private float rollSpeed = 1;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("Player damage amount.")]
	private int damage = 1;

	[Header("Energy Bar Stats")]
	[SerializeField]
	[Foldout("Player Stats"), Tooltip("Player max energy (the cooldown bar)")]
	private float maxEnergy = 10;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("Player current energy (the cooldown bar)")]
	private float currEnergy = 10.0f;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("Time it takes (in seconds) to replenish a portion of the energy bar.")]
	private float timer = 0;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("The rate (in seconds) at which energy is replenished.\nEx. A value of 1 means this player regains x energy points per 1 second.")]
	private float rate = 1;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("The amount of energy points that gets replenished per iteration.")]
	private float replenishAmount = 1;

	[Header("Dash Stats")]
	[SerializeField]
	[Foldout("Player Stats"), Tooltip("Player Dash speed.")]
	private int dashSpeed = 32;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("Player Dash duration.")]
	private float dashDuration = 0.125f;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("Player Dash duration.")]
	private float dashEnergyConsumption;

	[Header("Debuffs")]
	[SerializeField, ReadOnly]
	[Foldout("Player Stats"), Tooltip("The debuff that this player can give to other players.")]
	private Debuff giveableDebuff;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("The debuff that this player is currently suffering from.")]
	private Debuff inflictedDebuff;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("The debuff that this player is currently suffering from.")]
	private bool canShoot = true;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("The debuff that this player is currently suffering from.")]
	private bool chaosFactorCanShoot = true;

	[Header("Modifiers")]
	[SerializeField]
	[Foldout("Player Stats"), Tooltip("")]
	private List<Modifier> modifiersOnPlayer;

	[SerializeField, ReadOnly]
	[Foldout("Player Stats"), Tooltip("")]
	private bool canSelfDestruct = false;

	[SerializeField, ReadOnly]
	[Foldout("Player Stats"), Tooltip("")]
	private bool triShot = false;

	[SerializeField, ReadOnly]
	[Foldout("Player Stats"), Tooltip("")]
	private bool isPowerSaving = false;

	[SerializeField, ReadOnly]
	[Foldout("Player Stats"), Tooltip("")]
	private bool fragmentBullets = false;

	[SerializeField, ReadOnly]
	[Foldout("Player Stats"), Tooltip("")]
	private bool homingBullets = false;

	[SerializeField, ReadOnly]
	[Foldout("Player Stats"), Tooltip("")]
	private bool explodingBullets = false;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("How accurate a homing bullet would be to hitting its target.\nNOTE: A value of 0 means it will have a 0% chance to hit the target."), Range(0, 1f)]
	private float homingAccuracy = 0.8f;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("The speed of which a homing bullet will rotate at to aim towards it's target.\nNOTE: Lower speed values means it takes longer to adjust its rotation to face the target -> Less Accuracy")]
	private float homingBulletRotSpeed = 200f;

	[Header("General Stats")]
	[SerializeField]
	[Foldout("Rolling Stats")]
	private float rollingEnergyConsumption;
	[Header("Shield")]
	[SerializeField]
	[Foldout("Rolling Stats")]
	private float shieldScaleTime = 0.2f;
	[SerializeField]
	[Foldout("Rolling Stats")]
	private float shieldUpDuration = 8f;
	[SerializeField]
	[Foldout("Rolling Stats")]
	private float shieldMaxSize = 270f;

	[Header("Damage Boost")]
	[SerializeField]
	[Foldout("Rolling Stats")]
	private float damageBoostDuration = 7f;


	[Header("Speed Boost")]
	[SerializeField]
	[Foldout("Rolling Stats")]
	private float speedBoostDuration = 7f;

	#endregion Serialize Fields

	#region Private Variables
	private bool isDead = false;
	private Coroutine debuffCoroutine;
	private float nextFireTime = 0;
	private Texture playerColor;
	private Color uiColor;
	private float invincibilityTimer = 0f;
	#endregion Private Variables

	#region Getters & Setters
	public bool IsDead { get { return isDead; } set { isDead = value; } }
	public float InvincibilityTime { get { return invincibilityTime; } }
	public float MaxHealth { get { return maxHealth; } }
	public float CurrentHealth { get { return currHealth; } }
	public float MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }
	public float FireRate { get { return fireRate; } set { fireRate = value; } }
	public int Damage { get { return damage; } set { damage = value; } }
	public float MaxEnergy { get { return maxEnergy; } }
	public float CurrentEnergy { get { return currEnergy; } set { currEnergy = value; } }
	public int DashSpeed { get { return dashSpeed; } set { dashSpeed = value; } }
	public float Timer { get { return timer; } }
	public float Rate { get { return rate; } }
	public float DashDuration { get { return dashDuration; } }
	public float DashEnergyConsumption { get { return dashEnergyConsumption; } set { dashEnergyConsumption = value; } }
	public float NextFireTime { get { return nextFireTime; } set { nextFireTime = value; } }
	public Debuff GiveableDebuff { get { return giveableDebuff; } set { giveableDebuff = value; } }
	public Debuff InflictedDebuff
	{
		get { return inflictedDebuff; }
		set
		{
			// Debuff was inflicted on the player, activate the debuffs effects!
			inflictedDebuff = value;

			// If there isnt already a debuff running
			if (debuffCoroutine != null) return;

			// Start coroutine
			debuffCoroutine = StartCoroutine(ApplyDebuffEffects());
			// Activate debuff effects
			ActivateEffects();
		}
	}
	public bool CanShoot { get { return canShoot; } set { canShoot = value; } }
	public bool ChaosFactorCanShoot { get { return chaosFactorCanShoot; } set { chaosFactorCanShoot = value; } }
	public List<Modifier> ModifiersOnPlayer { get { return modifiersOnPlayer; } set { modifiersOnPlayer = value; } }
	public bool CanSelfDestruct { get { return canSelfDestruct; } set { canSelfDestruct = value; } }
	public float RollSpeed { get { return rollSpeed; } set { rollSpeed = value; GetComponent<PlayerBody>().HeadAnim["Roll"].speed = rollSpeed; GetComponent<PlayerBody>().LegAnim["Roll"].speed = rollSpeed; } }
	public bool TriShot { get { return triShot; } set { triShot = value; } }
	public bool IsPowerSaving { get { return isPowerSaving; } set { isPowerSaving = value; } }
	public bool FragmentBullets { get { return fragmentBullets; } set { fragmentBullets = value; } }
	public bool HomingBullets { get { return homingBullets; } set { homingBullets = value; } }
	public bool ExplodingBullets { get { return explodingBullets; } set { explodingBullets = value; } }
	public float HomingAccuracy { get { return homingAccuracy; } set { homingAccuracy = value; } }
	public float HomingBulletRotSpeed { get { return homingBulletRotSpeed; } set { homingBulletRotSpeed = value; } }

	// Rolling Stats
	public float RollingEnergyConsumption { get { return rollingEnergyConsumption; } set { rollingEnergyConsumption = value; } }
	public float ShieldScaleTime { get { return shieldScaleTime; } }
	public float ShieldUpDuration { get { return shieldUpDuration; } }
	public float ShieldMaxSize { get { return shieldMaxSize; } }
	public float DamageBoostDuration { get { return damageBoostDuration; } }
	public float SpeedBoostDuration { get { return speedBoostDuration; } }
	public Color UIColor { get { return uiColor; } set { uiColor = value; playerHUD.CharacterGlowColour = value; } }
	public float PlayerEmissionIntensity { get { return playerEmissionIntensity; } }
	public Texture PlayerColor
	{
		get { return playerColor; }
		set
		{
			playerColor = value;
			// Instantiate proper body parts
			GameObject head = GameObject.Instantiate(characterStat.playerModelHead, playerMeshGO.position, Quaternion.identity, playerMeshGO);
			GameObject legs = GameObject.Instantiate(characterStat.playerModelBody, playerLegGO.position, Quaternion.identity, playerLegGO);
			List<Material> listOfMaterials = new List<Material>();
			head.GetComponentInChildren<MeshRenderer>().GetMaterials(listOfMaterials);
			for (int i = 0; i < listOfMaterials.Count; i++)
			{
				if (listOfMaterials[i].name.Contains("Light"))
					listOfMaterials[i] = playerGlowMaterial;
			}
			head.GetComponentInChildren<MeshRenderer>().SetMaterials(listOfMaterials);

			// Left Leg
			listOfMaterials = new List<Material>();
			legs.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().GetMaterials(listOfMaterials);
			for (int i = 0; i < listOfMaterials.Count; i++)
			{
				if (listOfMaterials[i].name.Contains("Light"))
					listOfMaterials[i] = playerGlowMaterial;
			}
			legs.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().SetMaterials(listOfMaterials);

			// Right Leg
			listOfMaterials = new List<Material>();
			legs.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshRenderer>().GetMaterials(listOfMaterials);
			for (int i = 0; i < listOfMaterials.Count; i++)
			{
				if (listOfMaterials[i].name.Contains("Light"))
					listOfMaterials[i] = playerGlowMaterial;
			}
			legs.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshRenderer>().SetMaterials(listOfMaterials);

			ResetMaterialEmissionColor();
		}
	}
	public CharacterStatsSO CharacterStat
	{
		set
		{
			characterStat = value;

			// Assign proper stats
			maxHealth = characterStat.MaxHealth;
			movementSpeed = characterStat.DefaultMoveSpeed;
			fireRate = characterStat.DefaultFireRate;
			maxEnergy = characterStat.MaxEnergy;
			rate = characterStat.EnergyRegenRate;

			// Reset to Max
			currHealth = maxHealth;
			currEnergy = maxEnergy;

			playerHUD.CharacterGlow = characterStat.characterSprite;
			playerHUD.CharacterBG = characterStat.characterBGSprite;
		}
		get { return characterStat; }
	}
	#endregion Getters & Setters

	private void OnEnable()
	{
		ResetPlayer();
	}

	public void ResetMaterialEmissionColor()
	{
		// Finding how close to white the player's colour is
		// Note that it is only divided by 2 * 255 and not 3
		// This is only so that the amount of brightness taken from the emission is greater than 1 (has more effect).
		float colourBrightness = uiColor.r + uiColor.g + uiColor.b;
		colourBrightness /= (255 * 2);

		// Setting the player lights material values
		playerGlowMaterial.SetTexture("_EmissionMap", playerColor);
		playerGlowMaterial.SetColor("_BaseColor", uiColor);
		playerGlowMaterial.EnableKeyword("_EMISSION");
		playerGlowMaterial.SetColor("_EmissionColor", uiColor * (playerEmissionIntensity - colourBrightness)); // To convert from the regular colour to HDR (with intensity), multiply the intensity value into the colour. We subtract colourBrightness from intensity so the lighter colours aren't as blown out.

		// Setting the aiming UI material values
		playerAimUIMaterial.SetTexture("_EmissionMap", playerColor);
		playerAimUIMaterial.SetColor("_BaseColor", uiColor);
		playerAimUIMaterial.EnableKeyword("_EMISSION");
		playerAimUIMaterial.SetColor("_EmissionColor", uiColor * ((playerEmissionIntensity * 0.5f) - colourBrightness)); // Because the aim UI is thicker lined than the player lights, we're only going to consider half the intensity value. Again, removing colourBrightness from the emission to prevent blow out.
	}


	private void Update()
	{
		// Energy bar regen.
		if (Input.GetKeyDown(KeyCode.Q))
			ResetMaterialEmissionColor();

		if (invincibilityTimer > 0) invincibilityTimer -= Time.deltaTime;

		// Tick the timer
		timer += Time.deltaTime;

		// Ff timer is less than the rate needed to increae energy, return;
		if (timer < Rate) return;
		// Increase energy and reset timer
		currEnergy += replenishAmount;

		// If energy is maxed out, return;
		if (currEnergy > maxEnergy)
		{
			currEnergy = maxEnergy;
			return;
		}

		timer = 0;
	}

	private void DeactivateEffects(ParticleSystemStopBehavior behaviour) { burning.Stop(true, behaviour); }

	private void ActivateEffects() { burning.Play(); }

	public void TakeDamage(int amount, DamageType type)
	{
		// Make sure health never hits negative
		if (isDead) return;
		// Play damage sound
		if (!GetComponent<PlayerBody>().HasExploded)
		{
			AudioSource theSource = gameObject.GetComponentInChildren<AudioSource>();
			DamageSound(theSource);
		}

		if (invincibilityTimer > 0 && (type == DamageType.Bullet || type == DamageType.Falling)) return;
		invincibilityTimer = invincibilityTime;

		// Make sure health stays in the bounds
		if (currHealth - amount > 0) currHealth -= amount;
		else currHealth = 0;

		// If still has Hp no need to continue
		if (currHealth != 0) return;

		// If can self destruct dont start death (Not sure why, probably a separate coroutine?)
		if (canSelfDestruct)
		{
			GetComponent<PlayerBody>().InitiateSelfDestruct();
			return;
		}

		// Is truly dead
		isDead = true;
		StartDeath();
	}

	public void Heal(int healing)
	{
		if (currHealth < maxHealth)
			currHealth += healing;
	}

	public void StartDeath()
	{
		currHealth = 0;
		gameObject.GetComponent<Rigidbody>().useGravity = false;
		gameObject.GetComponent<PlayerBody>().Death();
	}

	public void UseEnergy(float amount)
	{
		currEnergy = currEnergy - amount;
		if (currEnergy > maxEnergy) { currEnergy = maxEnergy; }
		else if (currEnergy < 0) { currEnergy = 0; }
	}

	public void ActivateEffects(Modifier modifier)
	{
		modifier.AddEffects();
		modifiersOnPlayer.Add(modifier);
	}

	public void FullResetPlayer()
	{
		// Remove models
		for (int i = 0; i < playerMeshGO.childCount; i++) // Destroy the player Models attached to the player
			Destroy(playerMeshGO.GetChild(0).gameObject);

		// Reset modifiers
		if (modifiersOnPlayer.Count > 0)
		{
			foreach (Modifier modifier in modifiersOnPlayer)
			{
				modifier.RemoveEffects(this);
			}
			modifiersOnPlayer.Clear();
		}

		// Reset aspects of the body, such as aim direction, move direction, rotation, etc.
		GetComponent<PlayerBody>().ResetPlayer();
		// No need to reset stats
	}

	public void ResetPlayer()
	{
		DeactivateEffects(ParticleSystemStopBehavior.StopEmittingAndClear);
		currHealth = maxHealth;
		currEnergy = MaxEnergy;
		if (CanSelfDestruct) GetComponent<PlayerBody>().HasExploded = false;
	}

	/// <summary>
	/// This coroutine applies the debuff's effects throughout it's duration before removing itself from the player.
	/// </summary>
	/// <returns></returns>
	private IEnumerator ApplyDebuffEffects()
	{
		// Dealing damage as the debuff is still active.
		while (inflictedDebuff.debuffDuration > 0)
		{
			yield return new WaitForSeconds(inflictedDebuff.damageInterval);
			if (inflictedDebuff.shouldKill) currHealth -= inflictedDebuff.damage;
			else
			{
				if (currHealth - inflictedDebuff.damage < 1) currHealth = 1;
				else currHealth -= inflictedDebuff.damage;
			}
			inflictedDebuff.debuffDuration -= inflictedDebuff.damageInterval;
		}

		// Debuff duration has elapsed. No longer active, and thus, should be removed.
		inflictedDebuff = null;
		debuffCoroutine = null;
		DeactivateEffects(ParticleSystemStopBehavior.StopEmitting);
	}

	//Plays the player damage sound
	private void DamageSound(AudioSource audioSource)
	{
		float randPitch = UnityEngine.Random.Range(0.8f, 1.5f);
		audioSource.pitch = randPitch;
		AudioManager._Instance.PlaySoundFX(AudioManager._Instance.PlayerAudioList[1], audioSource);
	}
}
