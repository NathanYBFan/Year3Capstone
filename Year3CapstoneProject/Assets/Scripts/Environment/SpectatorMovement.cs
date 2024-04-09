using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;

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

        movementSpeed = movementSpeed * Random.Range(0.8f, 1.2f);


    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, originalY + ((float)Math.Sin(Time.time * movementSpeed) * 0.07f), transform.position.z);
    }
}