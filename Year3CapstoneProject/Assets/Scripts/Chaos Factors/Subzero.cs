using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subzero : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField]
    [Foldout("Floor Physics Material"), Tooltip("Physics material for the floor")]
    PhysicMaterial material;

    private float hold;

    private void OnEnable()
    {
        hold = material.dynamicFriction;
        material.dynamicFriction = 0;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnDestroy()
    {
        material.dynamicFriction = hold;
    }


}
