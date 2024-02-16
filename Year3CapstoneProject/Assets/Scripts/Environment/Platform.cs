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
    float time;
    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    float maxHeight;
    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    float minHeight;

    // Getters & Setters
    public GameObject IceTop { get { return iceTop; } set { iceTop = value; } }

    private void Awake()
    {
        time = 10;
        maxHeight = transform.position.y;
        minHeight = maxHeight - 20;
    }


    public void collapse()
    {
        //do visual thing as warning

        //
        StartCoroutine(Down());
        
    }
    public void rise()
    {
        //do visual thing as warning

        //
        Debug.Log("Rise called");
        StartCoroutine(Up());

    }

    public void fakeDestroy()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        StartCoroutine(destory());
        

    }

    public void fakeRespawn()
    {
        //Debug.Log("fake respawn called");
        StopAllCoroutines();
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
        StartCoroutine(Up());
    }

    public void toggleIce(bool i)
    {
        if (i == true) 
        { 
            snowBurst.Play(); 

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
        //Debug.Log("up called");
        float elapsedTime = 0;
       
        Vector3 endPos = new Vector3(transform.position.x, maxHeight, transform.position.z);
        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(transform.position, endPos, (elapsedTime /time));
            elapsedTime += 0.5f * Time.deltaTime;
            yield return null;
        }

        yield return null;
    }

    private IEnumerator destory()
    {
        //Debug.Log("down called");
        float elapsedTime = 0;
       
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(transform.position.x, minHeight, transform.position.z);
        while (elapsedTime < time) 
        {

            transform.position = Vector3.Lerp(transform.position, endPos, (elapsedTime / time));
            elapsedTime += 0.5f*Time.deltaTime;
            yield return null;
            //Debug.Log(elapsedTime);
        }
        fakeRespawn();
    }

    private IEnumerator Down()
    {
        //Debug.Log("down called");
        float elapsedTime = 0;

        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(transform.position.x, minHeight, transform.position.z);
        while (elapsedTime < time)
        {

            transform.position = Vector3.Lerp(transform.position, endPos, (elapsedTime / time));
            elapsedTime += 0.5f * Time.deltaTime;
            yield return null;
            //Debug.Log(elapsedTime);
        }
      
    }

}
