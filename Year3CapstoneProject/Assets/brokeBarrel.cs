using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brokeBarrel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, 3.0f);
        Debug.Log("Colliders hit: " + colliders.Length);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            //Debug.Log(hit.name);
            if (rb != null)
            {
                rb.AddExplosionForce(20f, transform.position, 3.0f, 1.0f, ForceMode.Force);

            }

        }
        StartCoroutine(boom());
    }

    private IEnumerator boom()
    {
        yield return new WaitForSeconds(6f);
        Destroy(gameObject);
    }

}
