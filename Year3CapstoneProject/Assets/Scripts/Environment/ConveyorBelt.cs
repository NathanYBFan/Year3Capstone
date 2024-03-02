using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField]
    [Foldout("Stats"), Tooltip("push force")]
    private float Cforce = 20f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.transform.parent.parent.GetComponent<Rigidbody>() != null && transform.parent.parent.GetComponent<Platform>().effectsActive == true)
        {
            Debug.Log("Zoom");
			Vector3 velocity = other.transform.parent.parent.GetComponent<Rigidbody>().velocity;
            PlayerBody body = other.transform.parent.parent.GetComponent<PlayerBody>();
            if (body == null) return;

            if (!body.CanMove)
			    other.transform.parent.parent.GetComponent<Rigidbody>().AddForce((transform.right * 5f) - velocity, ForceMode.VelocityChange);
            else
				other.transform.parent.parent.GetComponent<Rigidbody>().AddForce((transform.right * 5f), ForceMode.VelocityChange);
		}

    }
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") && other.transform.parent.parent.GetComponent<Rigidbody>() != null && transform.parent.parent.GetComponent<Platform>().effectsActive == true)
		{
			PlayerBody body = other.transform.parent.parent.GetComponent<PlayerBody>();
			if (body == null) return;
			if (!body.CanMove)
				other.transform.parent.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}


}
