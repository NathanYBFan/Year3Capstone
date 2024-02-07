using UnityEngine;

[CreateAssetMenu(fileName = "New Modifier", menuName = "Modifiers/Overcharged")]
public class Overcharged : Modifier
{
	public override void AddEffects()
	{
		PlayerStats playerStats = ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>();
		if (playerStats != null)
		{
			playerStats.MovementSpeed += 5;
			playerStats.FireRate += 5;
		}
	}

	public override void AddEffects(PlayerStats playerStats)
	{
		playerStats.MovementSpeed += 5;
		playerStats.FireRate += 5;
	}
}
