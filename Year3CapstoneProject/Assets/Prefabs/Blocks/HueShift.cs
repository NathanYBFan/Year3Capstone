using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HueShift : MonoBehaviour
{
	[SerializeField]
	private float minHue = 0;

	[SerializeField]
	private float maxHue = 260;

	[SerializeField]
	private float intensity = 2;

	[SerializeField]
	private float speed = 0.01f;
	[SerializeField]
	private Color currentColour;

	[SerializeField]
	private Material[] mats;

	private void Awake()
	{
		foreach (var mat in mats)
		{
			mat.EnableKeyword("_EMISSION");
			mat.SetColor("_EmissionColor", Color.red);
		}

	}
	
	void Update()
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
			cm.exposureValue = intensity;
			mat.SetColor("_EmissionColor", cm.exposureAdjustedColor);

		}
	}

}
