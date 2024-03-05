using UnityEngine;


[CreateAssetMenu(fileName = "New Modifier", menuName = "Modifiers/Fragmentation")]
public class Fragmentation : Modifier
{
	public override void AddEffects()
	{
		PlayerStats playerStats = ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>();
		if (playerStats != null)
		{
			playerStats.FragmentBullets = true;
		}
	}

	public override void AddEffects(PlayerStats playerStats)
	{
		playerStats.FragmentBullets = true;
	}

	public override void RemoveEffects()
	{
		PlayerStats playerStats = ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>();
		if (playerStats != null)
		{
			playerStats.FragmentBullets = false;
		}
	}

	public override void RemoveEffects(PlayerStats playerStats)
	{
		playerStats.FragmentBullets = false;
	}
}
