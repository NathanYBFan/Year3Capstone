using System.Collections;
using UnityEngine;

public class ExploiveBarrel : MonoBehaviour
{
    [SerializeField]
    private GameObject destroyedVersion;

    [SerializeField]
    private float explosiveForce;

    [SerializeField]
    private float explosiveRadius;

    private Vector3 expPos;
    private bool exploded;
    //private Rigidbody rb;

    private void Start()
    {
        //rb = destroyedVersion.GetComponent<Rigidbody>();
        expPos = transform.position;
        exploded = false;
        //Debug.Log(GetComponent<Rigidbody>().name);
    }

    private IEnumerator boom()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<BoxCollider>().enabled = false;    
        yield return new WaitForSeconds(6f);
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<SphereCollider>() != null && other.transform.GetComponent<SphereCollider>().CompareTag("Bullet") && !exploded)
        {
            GameObject dest = Instantiate(destroyedVersion, transform.position, transform.rotation, transform.parent.parent);

            GetComponent<MeshRenderer>().enabled = false;
            //GetComponent
            //transform.position = new Vector3(transform.position.x, transform.position.y - 20, transform.position.z);
            exploded = true;
            StartCoroutine(boom());
        }
    }
}
