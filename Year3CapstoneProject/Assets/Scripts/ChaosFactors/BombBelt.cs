using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBelt : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other != transform.parent && other.CompareTag("Player"))
        {
            transform.parent.GetComponent<ExplosiveTag>().swapTarget(other.transform.parent.parent.gameObject);

        }
    }
}
