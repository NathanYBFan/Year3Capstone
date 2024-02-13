using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explodeScript : MonoBehaviour
{
    public GameObject destroyedVersion;

    void OnMouseDown()
    {
        Instantiate(destroyedVersion, transform.position + new Vector3(2,0,2), transform.rotation);
        //Destroy(gameObject);
    }

}

//credits to: https://www.youtube.com/watch?v=EgNV0PWVaS8
