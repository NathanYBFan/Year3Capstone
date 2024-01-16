using UnityEngine;
using UnityEngine.UI;

public abstract class Modifier : MonoBehaviour
{
	public abstract string ModifierName { get; }

	public abstract Image ModifierImage { get; }
	public abstract string ModifierDescription { get; }

	/// <summary>
	/// This method will be used to implement the effects of the modifier on the player who has it.
	/// </summary>
	public abstract void AddEffects();
}
