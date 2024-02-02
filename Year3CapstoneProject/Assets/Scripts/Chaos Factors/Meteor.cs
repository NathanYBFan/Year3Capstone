using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEngine.UI.Image;

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

    [SerializeField]
    private GameObject fallMarker;

    [SerializeField]
    private float markerSpawnHeight;

    private GameObject markerInstance;

    void Awake()
    {
        transform.position = new Vector3(Random.Range(minSpawnX, maxSpawnX), spawnHeight, Random.Range(minSpawnZ, maxSpawnZ));
        // fallMarker.transform.position = new Vector3(transform.position.x, markerSpawnHeight, transform.position.z);
        markerInstance = Instantiate(fallMarker, new Vector3(transform.position.x, markerSpawnHeight+4, transform.position.z), transform.rotation);
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
        Destroy(markerInstance);
        MeteorVisual.enabled = false;
        GetComponentInChildren<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        yield return null;
    }




}
