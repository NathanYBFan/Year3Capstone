using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSaving : Modifier
{
	private string modifierName;
	public override string ModifierName
	{
		get { return modifierName; }
	}

	private void Start()
	{
		modifierName = "Power-Saving Mode";
	}

	public override void AddEffects()
	{
		PlayerStats playerStats = GetComponent<PlayerStats>();
		if (playerStats != null)
		{
			playerStats.isPowerSaving = true;
		}
	}
}
