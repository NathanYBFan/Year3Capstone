using UnityEngine;

[CreateAssetMenu(fileName = "New Modifier", menuName = "Modifiers/Power-Saving Mode")]
public class PowerSaving : Modifier
{
	public override void AddEffects()
	{
		PlayerStats playerStats = ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>();
		if (playerStats != null)
		{
			playerStats.IsPowerSaving = true;
		}
	}

	public override void AddEffects(PlayerStats playerStats)
	{
		playerStats.IsPowerSaving = true;
	}

	public override void RemoveEffects()
	{
		PlayerStats playerStats = ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>();
		if (playerStats != null)
		{
			playerStats.IsPowerSaving = false;
		}
	}

	public override void RemoveEffects(PlayerStats playerStats)
	{
		playerStats.IsPowerSaving = false;
	}
}
