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

	[SerializeField]
	private bool isDropped = false;


	public bool effectsActive;

	Coroutine destoryCoroutine = null;
	// Getters & Setters
	public GameObject IceTop { get { return iceTop; } set { iceTop = value; } }
	public bool IsDropped { get { return isDropped; } set { isDropped = value; } }

	private void Awake()
	{
		effectsActive = true;
		maxHeight = transform.position.y;
		minHeight = maxHeight - 40;


	}

	public void collapse()
	{
		//do visual thing as warning
		isDropped = true;
		StartCoroutine(Down());
	}
	public void rise()
	{
		//do visual thing as warning
		IsDropped = false;
		StartCoroutine(Up());
	}

	public void fakeDestroy(float delay)
	{
		isDropped = true;
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
        if (GetComponentInChildren<ExplosiveBarrel>() != null)
        {
            ExplosiveBarrel e = GetComponentInChildren<ExplosiveBarrel>();
            e.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        foreach (GameObject child in MiscChildren)
		{
			child.GetComponent<MeshRenderer>().enabled = false;
		}

		if (destoryCoroutine == null) destoryCoroutine = StartCoroutine(destory(delay));


	}

	public void fakeRespawn()
	{
		
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

        if (GetComponentInChildren<ExplosiveBarrel>() != null)
        {
            ExplosiveBarrel e = GetComponentInChildren<ExplosiveBarrel>();
            e.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }

        foreach (GameObject child in MiscChildren)
		{
			child.GetComponent<MeshRenderer>().enabled = true;
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
		if (collision.gameObject.tag == "test" && !isDropped)
		{
			
		}
	}

	public IEnumerator Up()
	{
		isDropped = true;
		Vector3 endPos = new Vector3(transform.position.x, maxHeight, transform.position.z);
		while (transform.position.y < endPos.y)
		{
			transform.position = Vector3.MoveTowards(transform.position, endPos, 20 * Time.deltaTime);			
			yield return null;
		}
		if (!iceTop.GetComponent<MeshRenderer>().enabled)
			effectsActive = true;
		else effectsActive = false;
		yield break;
	}

	private IEnumerator destory(float delay)
	{

		Vector3 endPos = new Vector3(transform.position.x, minHeight, transform.position.z);
		while (transform.position.y > endPos.y)
		{
			transform.position = Vector3.MoveTowards(transform.position, endPos, 20 * Time.deltaTime);
			yield return null;
		}
		yield return new WaitForSeconds(delay);
		fakeRespawn();
		destoryCoroutine = null;
		yield break;
	}

	private IEnumerator Down()
	{
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
