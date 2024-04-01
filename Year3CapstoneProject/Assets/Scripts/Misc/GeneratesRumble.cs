using NaughtyAttributes;
using UnityEngine;

public class GeneratesRumble : MonoBehaviour
{
	[SerializeField]
	[Foldout("Rumble Stats"), Tooltip("The time that the controller should rumble for.")]
	private float rumbleDuration;

	[SerializeField]
	[Range(0f, 1f)]
	[Foldout("Rumble Stats"), Tooltip("Speed for the left motor of the controller to move at.\nHigher values means greater rumble!")]
	private float leftIntensity;

	[SerializeField]
	[Range(0f, 1f)]
	[Foldout("Rumble Stats"), Tooltip("Speed for the right motor of the controller to move at.\nHigher values means greater rumble!")]
	private float rightIntensity;

	public float RumbleDuration { get { return rumbleDuration; } }
	public float LeftIntensity { get { return leftIntensity; } }
	public float RightIntensity { get { return rightIntensity; } }
}
