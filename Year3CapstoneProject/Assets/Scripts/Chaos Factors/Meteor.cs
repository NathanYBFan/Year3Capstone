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

    [SerializeField]
    private ParticleSystem explosion;

    void Awake()
    {
        transform.position = new Vector3(Random.Range(minSpawnX, maxSpawnX), spawnHeight, Random.Range(minSpawnZ, maxSpawnZ));
        rb = GetComponent<Rigidbody>();

    }


    // Update is called once per frame
    void Update()
    {
        rb.AddForce(transform.up*fallForce);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collsion");
        StartCoroutine(boom());
    }


    private IEnumerator boom()
    {
        Debug.Log("boom called");
        explosion.Play();
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
        yield return null;
    }




}
