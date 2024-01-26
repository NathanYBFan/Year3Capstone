using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{

    [SerializeField]
    private int spawnHeight;
    [SerializeField]
    private int maxSpawnX;
    [SerializeField]
    private int minSpawnX;
    [SerializeField]
    private int maxSpawnZ;
    [SerializeField]
    private int minSpawnZ;

    [SerializeField]
    private int fallForce;

    private Rigidbody rb;

    [SerializeField]
    private Collider Bigg;

    void Awake()
    {
        transform.position = new Vector3(Random.Range(minSpawnX, maxSpawnX), spawnHeight, Random.Range(minSpawnZ, maxSpawnZ));

        rb = GetComponent<Rigidbody>();



    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(transform.up*fallForce);
    }


    private void OnCollisionEnter(Collision collision)
    {
        
    }







}
