using UnityEngine;

[CreateAssetMenu(fileName = "New Modifier", menuName = "Modifiers/Overheated")]
public class Overheated : Modifier
{
	#region Serialize Fields
	[SerializeField]
	private Debuff debuffToApply;

	#endregion Serialize Fields

	public override void AddEffects()
	{
		PlayerStats playerStats = ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>();
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

	public override void AddEffects(PlayerStats playerStats)
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
