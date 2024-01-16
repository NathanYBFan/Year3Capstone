using UnityEngine;
using UnityEngine.UI;

public abstract class Modifier : ScriptableObject
{

	/// <summary>
	/// This method will be used to implement the effects of the modifier on the player who has it.
	/// </summary>
	public abstract void AddEffects();
}
