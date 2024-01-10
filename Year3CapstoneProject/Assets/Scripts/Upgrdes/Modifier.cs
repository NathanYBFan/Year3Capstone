using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modifier : MonoBehaviour
{
	public abstract string ModifierName { get; }

	/// <summary>
	/// This method will be used to implement the effects of the modifier on the player who has it.
	/// </summary>
	public abstract void AddEffects();
}
