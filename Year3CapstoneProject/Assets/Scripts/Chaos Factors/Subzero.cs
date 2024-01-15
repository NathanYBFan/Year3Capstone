using NaughtyAttributes;
using UnityEngine;

public class Subzero : MonoBehaviour
{
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Physics material for the floor")]
    PhysicMaterial material;

    private float hold;

    private void Start()
    {
        hold = material.dynamicFriction;
        material.dynamicFriction = 0;
        Debug.Log("Enable Function ran and finished");
    }

    private void OnDestroy()
    {
        material.dynamicFriction = hold;
        Debug.Log("On Destroy ran and finished");
    }
}
