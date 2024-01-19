using UnityEngine;


[CreateAssetMenu(fileName = "New Modifier", menuName = "Modifiers/Unstable Blast")]
public class UnstableBlast : Modifier
{
	public override void AddEffects()
	{
		PlayerStats playerStats = ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>();
		if (playerStats != null)
		{
			playerStats.explodingBullets = true;
		}
	}
}
