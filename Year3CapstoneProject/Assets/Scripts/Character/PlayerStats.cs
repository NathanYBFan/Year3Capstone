using System;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    [Foldout ("Player Stats"), Tooltip("Player max health")]
    private int maxHealth;
    [SerializeField]
    [Foldout("Player Stats"), Tooltip("Player current health")]
    private int currHealth;
    [SerializeField]
    [Foldout("Player Stats"), Tooltip("Player max movement speed")]
    private float movementSpeed;
    [SerializeField]
    [Foldout("Player Stats"), Tooltip("Player max firerate")]
    private float fireRate;
    [SerializeField]
    [Foldout("Player Stats"), Tooltip("Player max energy (the cooldown bar)")]
    private float maxEnergy;
    [SerializeField]
    [Foldout("Player Stats"), Tooltip("Player current energy (the cooldown bar)")]
    private float currEnergy;
    [SerializeField]
    [Foldout("Player Stats"), Tooltip("Time it takes to replenish energy bar")]
    private float timer;
    [SerializeField]
    [Foldout("Player Stats"), Tooltip("Rate at which the energy is replenished")]
    private float rate;

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
    }
    public float FireRate 
    { 
        get { return fireRate; } 
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
        if (currEnergy < maxEnergy)
        {
            timer += Time.deltaTime;
            if (timer >= Rate)
            {
                currEnergy++;
                timer = 0;
            }
        }
    }


}
