using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Platform : MonoBehaviour
{
    [SerializeField]
    float time;
    [SerializeField]
    private GameObject iceTop;

    [SerializeField]
    private GameObject iceTop2;


    [SerializeField]
    private ParticleSystem snowBurst;

    [SerializeField]
    private ParticleSystem snowBurst2;
    public GameObject IceTop { get { return iceTop; } set { iceTop = value; } }


    private void Awake()
    {
        time = 10;
    }

    public void fakeDestroy()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        StartCoroutine(Down());

    }


    public void fakeRespawn()
    {
        Debug.Log("fake respawn called");

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

    private IEnumerator Up()
    {
        //Debug.Log("up called");
        float elapsedTime = 0;
       
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(transform.position.x, transform.position.y + 10f, transform.position.z);
        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(transform.position, endPos, (elapsedTime /time));
            elapsedTime += 0.5f * Time.deltaTime;
            yield return null;
        }

        yield return null;
    }


    private IEnumerator Down()
    {
        //Debug.Log("down called");
        float elapsedTime = 0;
       
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(transform.position.x, transform.position.y-10f, transform.position.z);
        while (elapsedTime < time) 
        {

            transform.position = Vector3.Lerp(transform.position, endPos, (elapsedTime / time));
            elapsedTime += 0.5f*Time.deltaTime;
            yield return null;
            //Debug.Log(elapsedTime);
        }

        fakeRespawn();
    }

}
