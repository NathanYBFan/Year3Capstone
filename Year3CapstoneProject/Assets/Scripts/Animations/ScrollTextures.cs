using NaughtyAttributes;
using UnityEngine;

public class ScrollTextures : MonoBehaviour
{
	// Serialize Fields
	[SerializeField]
	[Foldout("Stats"), Tooltip("")]
	private float scrollX = 0.5f;

	[SerializeField]
	[Foldout("Stats"), Tooltip("")]
	private float scrollY = 0.5f;

	[SerializeField]
	private Material mat;

	private void Update()
	{
		if (transform.parent.GetComponent<Platform>().effectsActive == true)
		{
			float OffsetX = Time.time * scrollX;
			float OffsetY = Time.time * scrollY;
			mat.mainTextureOffset = new Vector2(OffsetX, OffsetY);
		}



	}
}
