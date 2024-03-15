using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HueShift : MonoBehaviour
{
	[SerializeField]
	private float maxIntensity = 2;

	[SerializeField]
	private float minIntensity = -4;

	[SerializeField]
	private float speed = 0.01f;
	[SerializeField]
	private Color currentColour;

	[SerializeField]
	private Material[] mats;

	[SerializeField]
	private Material[] stripLights;

	[SerializeField]
	private Material rampSurfaceMat;


	[SerializeField]
	private float scrollX = 0.5f;

	private void Awake()
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
			float currIntensity = maxIntensity;
			foreach (var mat in mats)
			{
				mat.EnableKeyword("_EMISSION");
				ColorMutator cm = new(Color.red);
				currIntensity = Mathf.Cos(Time.time / (speed * 3f)) * 2;
				cm.exposureValue = currIntensity;
				mat.SetColor("_EmissionColor", cm.exposureAdjustedColor);

			}
			foreach (var mat in stripLights)
			{
				float OffsetX = Time.time * scrollX;
				mat.mainTextureOffset = new Vector2(OffsetX, 0);
			}
		}
		else
		{
			foreach (var mat in mats)
			{
				float h, s, v;
				mat.EnableKeyword("_EMISSION");
				currentColour = mat.GetColor("_EmissionColor");
				Color.RGBToHSV(currentColour, out h, out s, out v);
				h += Time.deltaTime * speed;
				h %= 1;
				Color newColour = Color.HSVToRGB(h, s, v, true);

				ColorMutator cm = new(newColour);
				cm.exposureValue = maxIntensity;
				mat.SetColor("_EmissionColor", cm.exposureAdjustedColor);

			}
			foreach (var mat in stripLights)
			{
				float OffsetX = Time.time * scrollX;
				mat.mainTextureOffset = new Vector2(OffsetX, 0);
			}
		}
		
	}

}
