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
        get { return name; }
    }

	private void Start()
	{
		modifierName = "Overheated";
	}
	public override void AddEffects()
	{
		//Access the player stats
		//Add debuffToApply to a "giveableDebuffs" list
	}
}
