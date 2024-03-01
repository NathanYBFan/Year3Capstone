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
        Debug.Log(GetComponent<Rigidbody>().name);
    }

    void OnMouseDown()
    {
        GameObject dest = Instantiate(destroyedVersion, transform.position, transform.rotation);
     
        GetComponent<MeshRenderer>().enabled = false;



    }
    private IEnumerator boom()
    {
        yield return new WaitForSeconds(2f);
        //Destroy(gameObject);
    }
}
