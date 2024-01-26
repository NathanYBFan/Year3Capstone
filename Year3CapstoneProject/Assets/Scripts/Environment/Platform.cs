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

    private float minHight;
    private float maxHight;

    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startHight = this.transform.position.y;
    }


    // Start is called before the first frame update
    void Start()
    {
        fakeDestroy();
    }

    // Update is called once per frame
    void Update()
    {

        if (moveingUp)
        {
            Vector3 newPos = rb.position + (Vector3.up * speed * Time.deltaTime);
            if (newPos.y > maxHight)
            {
                newPos.y = maxHight;
                moveingUp = false;
                rb.MovePosition(newPos);

            }
        }

        if (moveingDown)
        {
            Vector3 newPos = rb.position + (Vector3.down * speed * Time.deltaTime);

            if (newPos.y < minHight)
            {
                newPos.y = maxHight; 
                moveingDown = false;
                rb.MovePosition(newPos);
            }
            
        }

    }


    public void fakeDestroy() 
    { 
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    
    }


    public void fakeRespawn() 
    {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;

    }


    public void moveUp() 
    { 
    
    
    }

    public void moveDown() 
    { 
    
    
    
    }

}
