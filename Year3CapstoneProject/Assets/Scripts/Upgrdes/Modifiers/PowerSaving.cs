using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "New Modifier", menuName = "Modifiers/Power-Saving Mode")]
public class PowerSaving : Modifier
{
	// Private Variables
	public string modifierName = "Power-Saving Mode";
	public Image modifierImage;
	public string modifierDescription;

	public override void AddEffects()
	{
		PlayerStats playerStats = ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>();
		if (playerStats != null)
		{
			playerStats.isPowerSaving = true;
		}
	}
}
