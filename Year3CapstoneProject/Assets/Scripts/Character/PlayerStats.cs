using System;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	[Header("Effects")]
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("The particle system prefabs for debuff effects")]
	private ParticleSystem burning;

	[Header("Character Stats")]
	[SerializeField]
	[Foldout("Player Stats"), Tooltip("Player max health")]			
	private int maxHealth = 100;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("Player current health")]		
	private int currHealth = 100;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("Player max move speed")]		
	private float movementSpeed = 10;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("Player Dash speed.")]
	private int dashSpeed = 250;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("Player max firerate")]		
	private float fireRate = 0;

	[Header("Energy Bar Stats")]
	[SerializeField]
	[Foldout("Player Stats"), Tooltip("Player max energy (the cooldown bar)")]		
	private float maxEnergy = 10;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("Player current energy (the cooldown bar)")]	
	private float currEnergy = 10;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("Time it takes (in seconds) to replenish a portion of the energy bar.")]
	private float timer = 0;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("The rate (in seconds) at which energy is replenished.\nEx. A value of 1 means this player regains x energy points per 1 second.")]
	private float rate = 1;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("The amount of energy points that gets replenished per iteration.")]
	private float replenishAmount = 1;

	[Header("Debuffs")]
	[SerializeField, ReadOnly]
	[Foldout("Player Stats"), Tooltip("The debuff that this player can give to other players.")]
	private Debuff giveableDebuff;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("The debuff that this player is currently suffering from.")]
	private Debuff inflictedDebuff;

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
	#endregion Serialize Fields

	#region Private Variables
	private bool isDead = false;
	private Coroutine debuffCoroutine;
	private float nextFireTime = 0;
	#endregion Private Variables
	
	#region Getters & Setters
	public bool IsDead { get { return isDead; } set { isDead = value; } }
	public int MaxHealth { get { return maxHealth; } }
	public int CurrentHealth { get { return currHealth; } }
	public float MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }
	public float FireRate { get { return fireRate; } set { fireRate = value; } }
	public float MaxEnergy { get { return maxEnergy; } }
	public float CurrentEnergy { get { return currEnergy; } set { currEnergy = value; } }
	public int DashSpeed { get { return dashSpeed; } set { dashSpeed = value; } }
	public float Timer { get { return timer; } }
	public float Rate { get { return rate; } }
	public float NextFireTime { get { return nextFireTime; } set { nextFireTime = value; } }
    public Debuff GiveableDebuff { get { return giveableDebuff; } set { giveableDebuff = value; } }
	public Debuff InflictedDebuff
	{ 
		get { return inflictedDebuff; }
		set 
		{
            // Debuff was inflicted on the player, activate the debuffs effects!
            inflictedDebuff = value;

			// If there was a debuff assigned
			if (inflictedDebuff != null) return;
			// If there isnt already a debuff running
			if (debuffCoroutine != null) return;
            
			// Start coroutine
			debuffCoroutine = StartCoroutine(ApplyDebuffEffects());
            // Activate debuff effects
			ActivateEffects();
        } 
	}
    public List<Modifier> ModifiersOnPlayer { get { return modifiersOnPlayer; } set { modifiersOnPlayer = value; } }
	public bool CanSelfDestruct { get { return canSelfDestruct; } set { canSelfDestruct = value; } }
	public bool TriShot { get { return triShot; } set { triShot = value; } }
	public bool IsPowerSaving { get { return isPowerSaving; } set { isPowerSaving = value; } }
    public bool FragmentBullets { get { return fragmentBullets; } set { fragmentBullets = value; } }
    public bool HomingBullets { get { return homingBullets; } set { homingBullets = value; } }
    public bool ExplodingBullets { get { return explodingBullets; } set { explodingBullets = value; } }
    public float HomingAccuracy { get { return homingAccuracy; } set { homingAccuracy = value; } }
	public float HomingBulletRotSpeed { get { return homingBulletRotSpeed; } set { homingBulletRotSpeed = value; } }
    public CharacterStatsSO CharacterStat
    {
        set
        {
            // Don't do work already done.
            if (characterStat == value) return;

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

			// Instantiate proper body parts
            GameObject.Instantiate(characterStat.playerModelHead, playerMeshGO.position, Quaternion.identity, playerMeshGO);
            GameObject.Instantiate(characterStat.playerModelBody, playerLegGO.position, Quaternion.identity, playerLegGO);
        }
    }
    #endregion Getters & Setters

    private void OnEnable()
	{
		DeactivateEffects(ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	private void Update()
	{
		// Energy bar regen.
		// If energy is maxed out, return;
		if (currEnergy >= maxEnergy) return;
		
		// Tick the timer
		timer += Time.deltaTime;
		
		// Ff timer is less than the rate needed to increae energy, return;
		if (timer < Rate) return;
		// Increase energy and reset timer
		currEnergy += replenishAmount;
		timer = 0;
	}

	private void DeactivateEffects(ParticleSystemStopBehavior behaviour) { burning.Stop(true, behaviour); }

	private void ActivateEffects()	{ burning.Play(); }

	public void TakeDamage(int amount)
	{
        // Make sure health never hits negative
        if (isDead) return;
		if (currHealth - amount > 0) currHealth -= amount;
		else currHealth = 0;

		// If still has Hp no need to continue
		if (!(currHealth == 0)) return;

        // If can self destruct dont start death (Not sure why, probably a separate coroutine?)
        if (canSelfDestruct) return; 
        
		// Is truly dead
		isDead = true;
        StartDeath();
    }
    
	private void StartDeath()
    {
        currHealth = 0;

        // Debug Death Message in logs.
        Debug.Log("Player " + (gameObject.GetComponent<PlayerBody>().PlayerIndex + 1) + " has died!");
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
		gameObject.GetComponent<PlayerBody>().Death();
    }

    public void UseEnergy(float amount)	{ currEnergy -= amount;	}

	public void ActivateEffects(Modifier modifier)
	{
		modifier.AddEffects();
        modifiersOnPlayer.Add(modifier);
	}

	public void ResetPlayer()
	{
		for (int i = 0; i < playerMeshGO.childCount; i++) // Destroy the player Models attached to the player
            Destroy(playerMeshGO.GetChild(0).gameObject);
		// No need to reset stats
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
}
