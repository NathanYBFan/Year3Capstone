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
			playerStats.RollingEnergyConsumption *= 0.5f;
			playerStats.DashEnergyConsumption *= 0.5f;
		}
	}

	public override void AddEffects(PlayerStats playerStats)
	{
		playerStats.IsPowerSaving = true;
		playerStats.RollingEnergyConsumption *= 0.5f;
		playerStats.DashEnergyConsumption *= 0.5f;
	}

	public override void RemoveEffects()
	{
		PlayerStats playerStats = ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>();
		if (playerStats != null)
		{
			playerStats.IsPowerSaving = false;
			playerStats.RollingEnergyConsumption *= 2f;
			playerStats.DashEnergyConsumption *= 2f;
		}
	}

	public override void RemoveEffects(PlayerStats playerStats)
	{
		playerStats.IsPowerSaving = false;
		playerStats.RollingEnergyConsumption *= 2f;
		playerStats.DashEnergyConsumption *= 2f;
	}
}
