using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overcharged : Modifier
{
	private string modifierName;
	public override string ModifierName
	{
		get { return modifierName; }
	}

	private void Start()
	{
		modifierName = "Overcharged";
	}

	public override void AddEffects()
	{
		PlayerStats playerStats = GetComponent<PlayerStats>();
		if (playerStats != null)
		{
			//TO DO -> Make movement speed and fire rate accessible somehow.
			playerStats.MovementSpeed += 5;
			playerStats.FireRate += 5;
		}
	}

}
