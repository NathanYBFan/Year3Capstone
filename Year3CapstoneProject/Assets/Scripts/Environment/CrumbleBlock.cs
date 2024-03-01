using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumbleBlock : MonoBehaviour
{
    [SerializeField]
    private float breakTime;

    [SerializeField]
    private float respawnTime;

    [SerializeField]
    Collider theCollider;

    [SerializeField]
    Collider theTrigger;

    public GameObject destroyedVersion;

    Platform thePlatform;

    

    MeshRenderer theMesh;

    // Start is called before the first frame update
    void Start()
    {
        Platform thePlatform = gameObject.GetComponent<Platform>();
        MeshRenderer theMesh = gameObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //If there is a player standing on this, call a timer coroutine that crumbles the block
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponentInChildren<CapsuleCollider>() != null && other.gameObject.GetComponentInChildren<CapsuleCollider>().CompareTag("Player"))
        {
            StartCoroutine(CrumbleTimer());
        }
            
    }

    //Timer coroutine that waits breakTime, crumbles the block, then re-instantiates it.
    private IEnumerator CrumbleTimer()
    {
        yield return new WaitForSeconds(breakTime);

        //Crumble the block
        Instantiate(destroyedVersion, transform.position, transform.rotation);
        if (gameObject != null)
        {
            thePlatform.collapse();
            theCollider.enabled = false;
            theMesh.enabled = false;
            theTrigger.enabled = false;
        }

        yield return new WaitForSeconds(respawnTime);

        //Re-appear the block and move it into place
        if (gameObject != null)
        {
            thePlatform.rise();
            theCollider.enabled = true;
            theMesh.enabled = true;
            theTrigger.enabled = true;
        }
    }
}
