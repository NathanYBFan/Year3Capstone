using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLightShow : MonoBehaviour
{

    [SerializeField]
    private float rotSpeed;

    [SerializeField]
    private int damage;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3 (30, 5.67f, 13);

    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(new Vector3(0, 0, rotSpeed) *Time.deltaTime);


    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<CapsuleCollider>() != null && other.CompareTag("Player"))
        {
            Debug.Log("Laser hit player");
            other.gameObject.transform.parent.parent.GetComponent<PlayerStats>().TakeDamage(damage);

        }

    }



}
