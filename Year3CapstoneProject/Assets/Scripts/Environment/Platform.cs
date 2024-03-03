using NaughtyAttributes;
using System.Collections;
using UnityEngine;
public class Platform : MonoBehaviour
{
	// Serialize Fields
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private GameObject iceTop;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private GameObject iceTop2;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private ParticleSystem snowBurst;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private ParticleSystem snowBurst2;

	[SerializeField]
	[Foldout("Stats"), Tooltip("")]
	float maxHeight;
	[SerializeField]
	[Foldout("Stats"), Tooltip("")]
	float minHeight;
	[SerializeField]
	GameObject[] MiscChildren;

	public bool effectsActive;

	// Getters & Setters
	public GameObject IceTop { get { return iceTop; } set { iceTop = value; } }

	private void Awake()
	{
		effectsActive = true;
		maxHeight = transform.position.y;
		minHeight = maxHeight - 20;
	}

	public void collapse()
	{
		//do visual thing as warning

		StartCoroutine(Down());
	}
	public void rise()
	{
		StopAllCoroutines();
		//do visual thing as warning

		StartCoroutine(Up());
	}

	public void fakeDestroy()
	{
		MeshRenderer r = GetComponent<MeshRenderer>();
		if (r != null )
		{
			r.enabled = false;
		}
		else
		{
			r = transform.GetComponentInChildren<MeshRenderer>();
			r.enabled = false;
		}
		GetComponent<Collider>().enabled = false;
		foreach (GameObject child in MiscChildren)
		{
			child.GetComponent<MeshRenderer>().enabled = false;
			child.GetComponent<Collider>().enabled = false;
		}
		StartCoroutine(destory());


	}

	public void fakeRespawn()
	{
		//Debug.Log("fake respawn called");
		StopAllCoroutines(); 
		MeshRenderer r = GetComponent<MeshRenderer>();
		if (r != null)
		{
			r.enabled = true;
		}
		else
		{
			r = transform.GetComponentInChildren<MeshRenderer>();
			r.enabled = true;
		}
		GetComponent<Collider>().enabled = true;
		foreach (GameObject child in MiscChildren)
		{
			child.GetComponent<MeshRenderer>().enabled = true;
			child.GetComponent<Collider>().enabled = true;
		}
		StartCoroutine(Up());
	}

	public void toggleIce(bool i)
	{
		effectsActive = !i;
		if (i == true)
		{

			if (snowBurst != null)
			{
				snowBurst.Play();
			}
			if (snowBurst2 != null)
			{
				snowBurst2.Play();
			}

		}
		iceTop.GetComponent<MeshRenderer>().enabled = i;

		if (iceTop2 != null)
		{
			iceTop2.GetComponent<MeshRenderer>().enabled = i;
		}

	}

	private void OnCollisionEnter(Collision collision)
	{

		if (collision.gameObject.tag == "test")
		{

			fakeDestroy();

		}
	}

	public IEnumerator Up()
	{
		Vector3 endPos = new Vector3(transform.position.x, maxHeight, transform.position.z);
		while (Vector3.Distance(transform.position, endPos) > 0.01f)
		{
			transform.position = Vector3.MoveTowards(transform.position, endPos, 20 * Time.deltaTime);
			
			yield return null;
		}
		effectsActive = true;
		yield return null;
	}

	private IEnumerator destory()
	{
		//Debug.Log("down called");

		Vector3 startPos = transform.position;
		Vector3 endPos = new Vector3(transform.position.x, minHeight, transform.position.z);
		while (Vector3.Distance(transform.position, endPos) > 0.01f)
		{
			transform.position = Vector3.MoveTowards(transform.position, endPos, 20 * Time.deltaTime);

			yield return null;
		}
		fakeRespawn();
	}

	private IEnumerator Down()
	{
		//Debug.Log("down called");
		effectsActive = false;
		Vector3 startPos = transform.position;
		Vector3 endPos = new Vector3(transform.position.x, minHeight, transform.position.z);
		while (Vector3.Distance(transform.position, endPos) > 0.01f)
		{
			transform.position = Vector3.MoveTowards(transform.position, endPos, 20 * Time.deltaTime);
			
			yield return null;
		}

	}

}
