using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HueShift : MonoBehaviour
{
	#region Serialize Fields
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private Material[] mats;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private Material[] stripLights;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private Material rampSurfaceMat;

	[SerializeField]
	private Color currentColour;

	[Header("Speed Stats")]
	[SerializeField]
	[Tooltip("How fast the lights shift in hue. \nNOTE: Higher values means faster speed.")]
	private float hueShiftSpeed = 0.05f;

	[SerializeField]
	[Tooltip("How slow the red lights blink during chaos factors. \nNOTE: Higher values means slower rate.")]
	private float redBlinkSlowness = 0.05f;

	[SerializeField]
	[Tooltip("Shortest amount of time that the transition between phases can occur.")]
	private float minTransitionSpeed = 1f;

	[SerializeField]
	[Tooltip("Longest amount of time that the transition between phases can occur.")]
	private float maxTransitionSpeed = 10f;

	[SerializeField]
	private float scrollX = 0.5f;

	[Header("Intensity Stats")]
	[SerializeField]
	[Tooltip("How bright the hue shift colours are.")]
	private float hueIntensity = 2;

	[SerializeField]
	[Tooltip("How bright the Red Blink is.")]
	private float redBlinkIntensity = 2;

	[SerializeField]
	[Tooltip("Shifts the range of numbers up or down (depending on the values given).\nEx. A value of -3 would lower all possible intensity values down by 3.")]
	private float redBlinkRangeModifier = 0;
	#endregion
	#region Private Variables
	private float currTime;
	private float currentIntensity;
	private float transitionSpeed = 3f;
	private bool lerpComplete = false;
	#endregion

	private void Awake()
	{
		InitializeMaterials();
	}
	private void InitializeMaterials()
	{
		foreach (var mat in mats)
		{
			mat.EnableKeyword("_EMISSION");
			mat.SetColor("_EmissionColor", Color.red);
		}
		foreach (var mat in stripLights)
		{
			mat.EnableKeyword("_EMISSION");
			mat.SetColor("_EmissionColor", Color.red);
			mat.mainTextureOffset = Vector2.zero;
		}

	}

	void Update()
	{
		if (ChaosFactorManager._Instance.ChaosFactorActive)
		{
			currentIntensity = Mathf.Lerp(currentIntensity, redBlinkIntensity, transitionSpeed * Time.deltaTime);
			currentColour = Color.Lerp(currentColour, Color.red, transitionSpeed * Time.deltaTime);

			if (!lerpComplete && Mathf.Abs(currentIntensity - redBlinkIntensity) < 0.01f)
			{
				lerpComplete = true;
				currTime = 0;
			}
			foreach (var mat in mats)
			{
				if (lerpComplete)
				{
					currentIntensity = redBlinkIntensity * Mathf.Cos(currTime / redBlinkSlowness) + redBlinkRangeModifier;
					currTime += Time.deltaTime;
				}
				ColorMutator cm = new(currentColour);
				cm.exposureValue = currentIntensity;
				mat.SetColor("_EmissionColor", cm.exposureAdjustedColor);
			}
		}
		else
		{
			lerpComplete = false;
			float distance = Mathf.Abs(hueIntensity - currentIntensity);
			transitionSpeed = Mathf.Lerp(minTransitionSpeed, maxTransitionSpeed, 1 - Mathf.Clamp01(distance / 5f)); // Adjust the range for smoother transitions
			currentIntensity = Mathf.Lerp(currentIntensity, hueIntensity, (transitionSpeed * Time.deltaTime) / distance);
			foreach (var mat in mats)
			{
				float h, s, v;
				currentColour = mat.GetColor("_EmissionColor");
				Color.RGBToHSV(currentColour, out h, out s, out v);
				h += Time.deltaTime * hueShiftSpeed;
				h %= 1;
				Color newColour = Color.HSVToRGB(h, s, v, true);

				ColorMutator cm = new(newColour);
				cm.exposureValue = currentIntensity;
				mat.SetColor("_EmissionColor", cm.exposureAdjustedColor);
			}
		}

		foreach (var mat in stripLights)
		{
			float OffsetX = Time.time * scrollX;
			mat.mainTextureOffset = new Vector2(OffsetX, 0);

		}



	}

}

