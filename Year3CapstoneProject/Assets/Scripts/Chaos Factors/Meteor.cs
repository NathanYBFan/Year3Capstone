using System.Collections;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField]
    private int damage;

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
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.GetComponentInChildren<CapsuleCollider>() != null && collision.gameObject.GetComponentInChildren<CapsuleCollider>().CompareTag("Player")) 
        {
            Debug.Log("player detected by meteor");
            collision.gameObject.transform.GetComponent<PlayerStats>().TakeDamage(damage);


        }

        StartCoroutine(boom());
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            UnityEngine.Debug.Log("player detected by meteor");
            other.transform.parent.parent.GetComponent<PlayerStats>().TakeDamage(10);


        }
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
