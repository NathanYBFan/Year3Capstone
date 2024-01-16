using UnityEngine;
using UnityEngine.UI;

public class Overheated : Modifier
{
	// Serialize Fields
    [SerializeField]
    private Debuff debuffToApply;

	// Private Variables
    private string modifierName = "Overheated";
	private Image modifierImage;
	private string modifierDescription;

	// Getters
    public override string ModifierName { get { return modifierName; } }
    public override Image ModifierImage { get { return modifierImage; } }
    public override string ModifierDescription { get {  return modifierDescription; } }

	public override void AddEffects()
	{
		PlayerStats playerStats = GetComponent<PlayerStats>();
		if (playerStats != null)
		{
			playerStats.giveableDebuff = new Debuff
			{
				debuffName = debuffToApply.name,
				debuffDuration = debuffToApply.debuffDuration,
				damageInterval = debuffToApply.damageInterval,
				damage = debuffToApply.damage,
				shouldKill = debuffToApply.shouldKill
			};
		}
	}
}
