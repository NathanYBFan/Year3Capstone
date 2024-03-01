using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField]
    [Foldout("Stats"), Tooltip("Direction the conveyorShouldPush")]
    private Vector3 moveDirection;


    private void OnTriggerStay(Collider other)
    {
        if ((other.transform.GetComponent<CapsuleCollider>() != null && other.transform.GetComponent<CapsuleCollider>().CompareTag("Player")))
        {
            Debug.Log(other.transform.parent.parent.name);
            other.transform.parent.parent.GetComponent<Rigidbody>().AddForce(transform.forward * 20f);
        }

    }


}
