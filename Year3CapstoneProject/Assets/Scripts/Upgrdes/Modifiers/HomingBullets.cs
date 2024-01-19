using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Modifier", menuName = "Modifiers/Homing Bullet")]
public class HomingBullets : Modifier
{
	public override void AddEffects()
	{
		PlayerStats playerStats = ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>();
		if (playerStats != null)
		{
			playerStats.homingBullets = true;
		}
	}
}
