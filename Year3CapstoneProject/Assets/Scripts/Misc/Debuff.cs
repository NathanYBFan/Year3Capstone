using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Debuff", menuName = "Status Effects/Debuff")]
public class Debuff : ScriptableObject
{
	public string debuffName;
	public float debuffDuration;
	public float damageInterval; //If this debuff damages, this is how quickly  its damage should be applied
	public float damage; //The amount of damage this debuff does per damageInterval
	public bool shouldKill; //Should this debuff be able to kill the entity?
}
