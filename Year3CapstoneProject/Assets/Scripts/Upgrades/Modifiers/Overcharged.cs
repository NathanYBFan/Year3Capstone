using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "New Modifier", menuName = "Modifiers/Overcharged")]
public class Overcharged : Modifier
{
	// Private Variables


	public override void AddEffects()
	{
		PlayerStats playerStats = ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>();
		if (playerStats != null)
		{
			//TO DO -> Make movement speed and fire rate accessible somehow.
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
