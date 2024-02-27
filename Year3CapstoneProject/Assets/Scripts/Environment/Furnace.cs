using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour
{
    [SerializeField]
    private bool isOn;

    [SerializeField]
    private ParticleSystem[] flameArray;

    [SerializeField]
    private int damage;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OnOffCycle());
    }

    // In update, handle turning the flame effect on and off visually.
    private void Update()
    {
        if (isOn)
        {
            for(int i = 0; i < flameArray.Length; i++)
            {
                if (!flameArray[i].isPlaying)
                {
                    flameArray[i].Play();
                }
               
            }
            
            StartCoroutine(OnOffCycle());
        }
        else if (isOn == false)
        {

            for (int i = 0; i < flameArray.Length; i++)
            {
                flameArray[i].Stop(true, ParticleSystemStopBehavior.StopEmitting);

            }
                
        }
        
    }


    void OnCollisionEnter(Collision collision)
    {
        //if the colliding object is a player (check tag), try to deal damage
        if(collision.gameObject.GetComponentInChildren<CapsuleCollider>() != null && collision.gameObject.GetComponentInChildren<CapsuleCollider>().CompareTag("Player"))
        {
            //damage the player that made contact, only if the fire is on
            if (isOn)
            {
                collision.gameObject.transform.GetComponent<PlayerStats>().TakeDamage(damage);
            }
            
        }

    }

    //alternates the furnace between being on for 3s, then off for 5s
    private IEnumerator OnOffCycle()
    {
        if (isOn)
        {
            yield return new WaitForSeconds(3f);
            //turn off fire
            isOn = false;
        }
        yield return new WaitForSeconds(5f);
        //turn on fire
        isOn = true;



    }
}
