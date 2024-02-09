using System;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
	#region Serialize Fields
	[SerializeField, ReadOnly]
	[Foldout("Dependencies"), Tooltip("")]	private CharacterStatsSO characterStat;
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]	private Transform playerMeshGO;
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]	private Transform playerLegGO;

	[Header("Effects")]
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]	private ParticleSystem burning; // The particle system prefabs for debuff effects

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
	public Debuff giveableDebuff;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("The debuff that this player is currently suffering from.")]
	public Debuff inflictedDebuff;

	[Header("Modifiers")]
	[SerializeField]
	[Foldout("Player Stats"), Tooltip("")]	public List<Modifier> modifiers;
	[ReadOnly]
	[Foldout("Player Stats"), Tooltip("")]	public bool canSelfDestruct = false;
	[ReadOnly]
	[Foldout("Player Stats"), Tooltip("")]	public bool triShot = false;
	[ReadOnly]
	[Foldout("Player Stats"), Tooltip("")]	public bool isPowerSaving = false;
	[ReadOnly]
	[Foldout("Player Stats"), Tooltip("")]	public bool fragmentBullets = false;
	[ReadOnly]
	[Foldout("Player Stats"), Tooltip("")]	public bool homingBullets = false;
	[ReadOnly]
	[Foldout("Player Stats"), Tooltip("")]	public bool explodingBullets = false;

	[SerializeField]
	[Foldout("Player Stats"), Tooltip("How accurate a homing bullet would be to hitting its target.\nNOTE: A value of 0 means it will have a 0% chance to hit the target."), Range(0, 1f)]
	public float homingAccuracy = 0.8f;
	[SerializeField]
	[Foldout("Player Stats"), Tooltip("The speed of which a homing bullet will rotate at to aim towards it's target.\nNOTE: Lower speed values means it takes longer to adjust its rotation to face the target -> Less Accuracy")]
	public float homingBulletRotSpeed = 200f;
	#endregion Serialize Fields
	#region Private Variables
	private bool isDead = false;
	private Coroutine debuffCoroutine;
	bool messageSent = false;
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
	#endregion Getters & Setters

	private void OnEnable()
	{
		DeactivateEffects(ParticleSystemStopBehavior.StopEmittingAndClear);
	}
	private void Update()
	{
		// Energy bar regen.
		if (currEnergy < maxEnergy)
		{
			timer += Time.deltaTime;
			if (timer >= Rate)
			{
				currEnergy += replenishAmount;
				timer = 0;
			}
		}

		// Debuff was inflicted on the player, activate the debuffs effects!
		if (inflictedDebuff != null)
		{
			if (debuffCoroutine == null)
			{
				debuffCoroutine = StartCoroutine(ApplyDebuffEffects());
				ActivateEffects();
			}
		}

		// Death.
		if (currHealth <= 0)
		{
			currHealth = 0;
			if (!canSelfDestruct)
			{
				isDead = true;
				StartDeath();
			}
		}

		// Debug Death Message in logs.
		if (isDead && !messageSent)
		{
			Debug.Log("Player " + gameObject.GetComponent<PlayerBody>().PlayerIndex + " has died!");
			messageSent = true;
		}
	}

	private void DeactivateEffects(ParticleSystemStopBehavior behaviour)	{		burning.Stop(true, behaviour);	}

	private void ActivateEffects()	{		burning.Play();	}

	public void SetCharacterStats(CharacterStatsSO newCharacterStat)
	{
		// Don't do work already done.
		if (newCharacterStat == characterStat) return;
		characterStat = newCharacterStat;

		// Instantiate model.
		maxHealth = newCharacterStat.MaxHealth;
		movementSpeed = newCharacterStat.DefaultMoveSpeed;
		fireRate = newCharacterStat.DefaultFireRate;
		maxEnergy = newCharacterStat.MaxEnergy;
		rate = newCharacterStat.EnergyRegenRate;
	}

	public void StartDeath()
	{
		currHealth = 0;
		isDead = true;

		gameObject.GetComponent<PlayerBody>().Death();
	}

	public void TakeDamage(int amount)
	{
		if (currHealth - amount > 0) currHealth -= amount;
		else currHealth = 0;
	}
	public void UseEnergy(float amount)	{		currEnergy -= amount;	}

	public void ActivateEffects(Modifier modifier)
	{
		modifier.AddEffects();
		modifiers.Add(modifier);
	}

	public void ApplyStats(CharacterStatsSO charStat)
	{
		maxHealth = charStat.MaxHealth;
		movementSpeed = charStat.DefaultMoveSpeed;
		fireRate = charStat.DefaultFireRate;
		maxEnergy = charStat.MaxEnergy;
		rate = charStat.EnergyRegenRate;

		currHealth = maxHealth;
		currEnergy = maxEnergy;

		GameObject.Instantiate(charStat.playerModelHead, playerMeshGO.position, Quaternion.identity, playerMeshGO);
		GameObject.Instantiate(charStat.playerModelBody, playerLegGO.position, Quaternion.identity, playerLegGO);
	}

	public void ResetPlayer()
	{
		for (int i = 0; i < playerMeshGO.childCount; i++) // Destroy the player Models attached to the player
            Destroy(playerMeshGO.GetChild(0).gameObject);

		// No need to reset stats?
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
