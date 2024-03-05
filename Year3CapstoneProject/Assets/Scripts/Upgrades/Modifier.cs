using UnityEngine;
using UnityEngine.UI;

public abstract class Modifier : ScriptableObject
{
	public string modifierName;

	public Sprite modifierImage;

	public string modifierDescription;


	/// <summary>
	/// This method will be used to implement the effects of the modifier on the player who has it.
	/// </summary>
	public abstract void AddEffects();
	/// <summary>
	/// This method will be used to implement the effects of the modifier on the player who has it.
	/// </summary>
	public abstract void AddEffects(PlayerStats playerStats);
}
