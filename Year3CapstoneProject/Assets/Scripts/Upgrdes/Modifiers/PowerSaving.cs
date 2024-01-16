using UnityEngine.UI;

public class PowerSaving : Modifier
{
	// Private Variables
	private string modifierName = "Power-Saving Mode";
	private Image modifierImage;
	private string modifierDescription;

	// Getters
	public override string ModifierName{ get { return modifierName; } }
	public override Image ModifierImage { get { return modifierImage; } }
	public override string ModifierDescription { get {  return modifierDescription; } }

	public override void AddEffects()
	{
		PlayerStats playerStats = GetComponent<PlayerStats>();
		if (playerStats != null)
		{
			playerStats.isPowerSaving = true;
		}
	}
}
