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

    [SerializeField]
    private MeshRenderer MeteorVisual;

    private Rigidbody rb;

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
        StartCoroutine(boom());
    }


    private IEnumerator boom()
    {
        explosion.Play();
        MeteorVisual.enabled = false;
        GetComponentInChildren<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        yield return null;
    }




}
