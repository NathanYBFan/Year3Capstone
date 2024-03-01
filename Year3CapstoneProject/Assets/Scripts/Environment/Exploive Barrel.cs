using System.Collections;
using System.Collections.Generic;
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

    //private Rigidbody rb;

    private void Start()
    {
        //rb = destroyedVersion.GetComponent<Rigidbody>();
        expPos = transform.position;
        //Debug.Log(GetComponent<Rigidbody>().name);
    }

    private IEnumerator boom()
    {
        yield return new WaitForSeconds(6f);
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        if ((other.transform.GetComponent<SphereCollider>() != null && other.transform.GetComponent<SphereCollider>().CompareTag("Bullet")))
        {
            GameObject dest = Instantiate(destroyedVersion, transform.position, transform.rotation);

            GetComponent<MeshRenderer>().enabled = false;
            //transform.position = new Vector3(transform.position.x, transform.position.y - 20, transform.position.z);
            StartCoroutine(boom());
           

           

        }
    }


}
