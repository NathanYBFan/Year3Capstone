using UnityEngine;

[CreateAssetMenu(fileName = "New Modifier", menuName = "Modifiers/Overcharged")]
public class Overcharged : Modifier
{
	public override void AddEffects()
	{
		PlayerStats playerStats = ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>();
		if (playerStats != null)
		{
			playerStats.MovementSpeed += 3;
			playerStats.RollSpeed = 3;
			//playerStats.FireRate += .5f;
		}
	}

	public override void AddEffects(PlayerStats playerStats)
	{
		playerStats.MovementSpeed += 3;
		playerStats.RollSpeed = 3;
		//playerStats.FireRate += .5f;
	}

	public override void RemoveEffects()
	{
		PlayerStats playerStats = ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>();
		if (playerStats != null)
		{
			playerStats.MovementSpeed -= 3;
			playerStats.RollSpeed = 2;
			//playerStats.FireRate -= .5f;
		}
	}

	public override void RemoveEffects(PlayerStats playerStats)
	{
		playerStats.MovementSpeed -= 3;
		playerStats.RollSpeed = 2;
		//playerStats.FireRate -= .5f;
	}
}
