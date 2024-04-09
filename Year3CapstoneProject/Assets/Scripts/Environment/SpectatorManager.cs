using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpectatorManager : MonoBehaviour
{
    [SerializeField]
    private float movementCap;

    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    GameObject Blue;

    [SerializeField]
    GameObject Orange;

    [SerializeField]
    GameObject Yellow;

    [SerializeField]
    GameObject Purple;

    [SerializeField]
    GameObject Green;

    GameObject[] parents;

    GameObject[,] spectatorsObjs;

    float[,] startYs;

    float[,] movementSpeeds;

    

    void Start()
    {


        //this.originalY = this.transform.position.y;

        //movementSpeed = movementSpeed * Random.Range(0.5f, 1.5f);

        //init arrays
        parents = new GameObject[5];

        parents[0] = Blue;
        parents[1] = Orange;
        parents[2] = Yellow;
        parents[3] = Purple;
        parents[4] = Green;


        spectatorsObjs = new GameObject[5,18];
        startYs = new float[5,18];
        movementSpeeds = new float[5,18];


        //read into arrays

        for(int i = 0; i < parents.Length; i++) 
        { 
        
            for(int j = 0; j < 18; j++) 
            {
                spectatorsObjs[i,j] = parents[i].transform.GetChild(j).gameObject;
                startYs[i, j] = spectatorsObjs[i, j].transform.position.y;
                movementSpeeds[i,j] = movementSpeed * Random.Range(0.5f, 1.5f);


            }
        
        }
        
        



    }

    void Update()
    {


        for (int i = 0; i < parents.Length; i++)
        {

            for (int j = 0; j < 18; j++)
            {
                spectatorsObjs[i,j].transform.position = new Vector3(transform.position.x, startYs[i, j] + ((float)Math.Sin(Time.time * movementSpeeds[i, j]) * 0.25f), transform.position.z);


            }

        }


 
    }
}
