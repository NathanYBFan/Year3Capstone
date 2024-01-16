using System;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

	[Header("Character Stats")]
	[SerializeField]
    [Foldout ("Player Stats"), Tooltip("Player max health")]
    private int maxHealth = 100;
    [SerializeField]
    [Foldout("Player Stats"), Tooltip("Player current health")]
    private int currHealth = 100;
    [SerializeField]
    [Foldout("Player Stats"), Tooltip("Player max movement speed")]
    private float movementSpeed = 10;
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
	[SerializeField, ReadOnly]
	[Foldout("Player Stats"), Tooltip("The debuff that this player is currently suffering from.")]
	public Debuff inflictedDebuff;

    [Header("Debuffs")]
    [SerializeField, ReadOnly]
    public List<Modifier> modifiers;
	private Coroutine debuffCoroutine;

    [ReadOnly]
    public bool isPowerSaving = false;
    public int MaxHealth
    {
        get { return maxHealth; }
    }
    public int CurrHealth
    {
        get { return currHealth; }
    }
    public float MovementSpeed
    {
        get { return movementSpeed; }
        set { movementSpeed = value; }
    }
    public float FireRate 
    { 
        get { return fireRate; } 
        set {  fireRate = value; }
    }
    public float MaxEnergy 
    { 
        get { return maxEnergy; } 
    }
    public float CurrentEnergy 
    { 
        get { return currEnergy; } 
    }
    public float Timer 
    { 
        get { return timer; } 
    }
    public float Rate 
    { 
        get { return rate; } 
    }

    private void Update()
    {
        //Energy bar regeneration.
        if (currEnergy < maxEnergy)
        {
            timer += Time.deltaTime;
            if (timer >= Rate)
            {
                currEnergy += replenishAmount;
                timer = 0;
            }
        }

        //Debuff was inflicted on the player, activate the debuffs effects!
        if (inflictedDebuff != null)
        {
            if (debuffCoroutine == null)
            {
                debuffCoroutine = StartCoroutine(ApplyDebuffEffects());
            }
        }
    }

    public void ActivateEffects(Modifier modifier)
    {
        modifier.AddEffects();
        modifiers.Add(modifier);
    }
    /// <summary>
    /// This coroutine applies the debuff's effects throughout it's duration before removing itself from the player.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ApplyDebuffEffects()
    {
        while (inflictedDebuff.debuffDuration > 0)
        {
            yield return new WaitForSeconds(inflictedDebuff.damageInterval);
			if (inflictedDebuff.shouldKill) currHealth -= inflictedDebuff.damage;
            else
            {
                if (currHealth - inflictedDebuff.damage < 1) currHealth = 1;
            }
            inflictedDebuff.debuffDuration -= inflictedDebuff.damageInterval;
		}
        inflictedDebuff = null;
    }
}
