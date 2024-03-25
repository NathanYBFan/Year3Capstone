using UnityEngine;
using System;
using System.Collections;

public class SpectatorMovement : MonoBehaviour
{ 
    [SerializeField]
    private float movementCap;

    [SerializeField]
    private float movementSpeed;

    float originalY;

    void Start()
    {
        this.originalY = this.transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, originalY + ((float)Math.Sin(Time.time * movementSpeed) * movementCap), transform.position.z);
    }
}