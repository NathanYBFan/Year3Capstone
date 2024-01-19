using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "New Modifier", menuName = "Modifiers/Fragmentation")]
public class Fragmentation : Modifier
{
	public override void AddEffects()
	{
		PlayerStats playerStats = ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>();
		if (playerStats != null)
		{
			playerStats.fragmentBullets = true;
		}
	}

}
