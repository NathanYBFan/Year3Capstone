using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overheated : Modifier
{
    [SerializeField]
    private Debuff debuffToApply;


    private string modifierName;
    public override string ModifierName
    {
        get { return modifierName; }
    }

	private void Start()
	{
		modifierName = "Overheated";
	}
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
