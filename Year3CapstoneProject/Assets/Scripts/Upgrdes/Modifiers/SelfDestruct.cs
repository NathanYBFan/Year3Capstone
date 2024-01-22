using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Modifier", menuName = "Modifiers/You're Going Down With Me")]
public class SelfDestruct : Modifier
{
	public override void AddEffects()
	{
		PlayerStats playerStats = ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>();
		if (playerStats != null)
		{
			playerStats.canSelfDestruct = true;
		}
	}

	public override void AddEffects(PlayerStats playerStats)
	{
		playerStats.canSelfDestruct = true;
	}
}
