using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField]
    [Foldout("Stats"), Tooltip("puch force")]
    private float Cforce = 5f;

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.GetComponent<CapsuleCollider>() != null && other.transform.GetComponent<CapsuleCollider>().CompareTag("Player") && other.transform.parent.parent.GetComponent<Rigidbody>() != null && transform.parent.parent.GetComponent<Platform>().effectsActive == true)
        {
            Debug.Log("");
            other.transform.parent.parent.GetComponent<Rigidbody>().AddForce(transform.right * 5f, ForceMode.VelocityChange);
        }

    }


}
