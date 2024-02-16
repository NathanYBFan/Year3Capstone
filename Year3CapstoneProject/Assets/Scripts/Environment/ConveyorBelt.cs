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
        Debug.Log(other.gameObject.name);
        if (other.GetComponentInChildren<CapsuleCollider>() == null) return;

        if (!other.GetComponentInChildren<CapsuleCollider>().CompareTag("Player")) return;

        other.gameObject.transform.parent.parent.GetComponent<Rigidbody>().AddForce(moveDirection * Time.deltaTime);
    }
}
