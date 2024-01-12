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
			//TO DO -> Make a bool on player stats that says whether power saving is enabled.
			//In PlayerStats.cs -> Do a check if this bool true. If true, the decrements from energy OnDash/OnRoll will be less than the regular ofc.
			//Either that, or we just increase the energy bar size... above comment though would be more 1:1 to what we intended though.
			//playerStats.isPowerSaving = true;
		}
	}
}
