using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explodeScript : MonoBehaviour
{
    public GameObject destroyedVersion;


    //credits to: https://www.youtube.com/watch?v=EgNV0PWVaS8
    void OnMouseDown()
    {
        Instantiate(destroyedVersion, transform.position, transform.rotation);
        if(gameObject != null)
        {
            Destroy(gameObject);
        }
        
    }

}


