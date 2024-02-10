using UnityEngine;

[CreateAssetMenu(fileName = "New Modifier", menuName = "Modifiers/Trifecta")]
public class Trifecta : Modifier
{
	public override void AddEffects()
	{
		PlayerStats playerStats = ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>();
		if (playerStats != null)
		{
			playerStats.TriShot = true;
		}
	}

	public override void AddEffects(PlayerStats playerStats)
	{
		playerStats.TriShot = true;
	}
}
