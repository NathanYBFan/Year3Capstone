using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
	private Vector3 contactNormal = Vector3.up;
	public Vector3 ContactNormal { get { return contactNormal; } }

	private void OnCollisionEnter(Collision collision)	{		EvaluateCollision(collision);	}
	private void OnCollisionStay(Collision collision)	{		EvaluateCollision(collision);	}
	private void OnCollisionExit(Collision collision)	{		contactNormal = Vector3.up;		}
	
	void EvaluateCollision(Collision collision)
	{
		for (int i = 0; i < collision.contactCount; i++)
		{
			contactNormal = collision.GetContact(i).normal;
		}
	}
}
