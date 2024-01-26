using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Platform : MonoBehaviour
{

    [SerializeField]
    float speed;


    private bool moveingUp = false;
    private bool moveingDown = false;

    private float startHight;

    [SerializeField]
    private float minHight;
    [SerializeField]
    private float maxHight;

    //private Rigidbody rb;


    private void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        //startHight = this.transform.position.y;
    }


    // Start is called before the first frame update
    void Start()
    {
        //moveUp();
    }

    // Update is called once per frame
    void Update()
    {

        //if (moveingUp)
        //{
        //    Vector3 newPos = rb.position + (Vector3.up * speed * Time.deltaTime);
        //    if (newPos.y > maxHight)
        //    {
        //        newPos.y = maxHight;
        //        moveingUp = false;
        //        rb.MovePosition(newPos);

        //    }
        //}



    //    if (moveingDown)
    //    {
    //        Vector3 newPos = rb.position + (Vector3.down * speed * Time.deltaTime);

    //        if (newPos.y < minHight)
    //        {
    //            newPos.y = minHight; 
    //            moveingDown = false;
    //            rb.MovePosition(newPos);
    //        }
            
    //    }

    }



    public void fakeDestroy()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

    }


    //public void fakeRespawn() 
    //{
    //    GetComponent<MeshRenderer>().enabled = true;
    //    GetComponent<Collider>().enabled = true;
    //}


    //public void moveUp() 
    //{
    //    Debug.Log("MoveUp called");
    //    moveingUp = true;
    //    Debug.Log(moveingUp);

    //}

    //public void moveDown() 
    //{
    //    moveingDown = true;
    //}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collsion detected");
        if (collision.gameObject.tag == "test")
        {
            Debug.Log("if entered");
            fakeDestroy();


        }

    }






}
