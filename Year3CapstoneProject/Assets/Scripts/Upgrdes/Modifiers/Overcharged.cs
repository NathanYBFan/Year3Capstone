using UnityEngine.UI;

public class Overcharged : Modifier
{
	// Private Variables
	private string modifierName = "Overcharged";

	public Image modifierImage;

	public string modifierDescription;

	// Getters
	public override string ModifierName { get { return modifierName; } }
	public override Image ModifierImage { get { return modifierImage; } }
    public override string ModifierDescription { get { return modifierDescription; } }

	public override void AddEffects()
	{
		PlayerStats playerStats = GetComponent<PlayerStats>();
		if (playerStats != null)
		{
			//TO DO -> Make movement speed and fire rate accessible somehow.
			playerStats.MovementSpeed += 5;
			playerStats.FireRate += 5;
		}
	}

}
