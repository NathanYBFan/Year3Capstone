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

    [SerializeField]
    private float delay;

    [SerializeField]
    private float burnTime;

    //not to be confused with damage, this is whether or not a player has been damaged
    private bool damaged;

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
            for (int i = 0; i < flameArray.Length; i++)
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


    void OnTriggerStay(Collider other)
    {
        //if the colliding object is a player (check tag), try to deal damage
        if (other.gameObject.GetComponentInChildren<CapsuleCollider>() != null && other.gameObject.GetComponentInChildren<CapsuleCollider>().CompareTag("Player"))
        {
            //damage the player that made contact, only if the fire is on
            if (isOn && !damaged)
            {
                other.transform.parent.parent.GetComponent<PlayerStats>().TakeDamage(damage, DamageType.Hazard);
                damaged = true;
                StartCoroutine(AllowDamage());
            }

        }

    }

    //if damaged is true, wait a second and then turn it false
    private IEnumerator AllowDamage()
    {
        if (damaged)
        {
            yield return new WaitForSeconds(1f);
            damaged = false;
        }
    }

    //alternates the furnace between being on for (burnTime), then off for (delay)
    private IEnumerator OnOffCycle()
    {
        if (isOn)
        {
            yield return new WaitForSeconds(burnTime);
            //turn off fire
            isOn = false;
            damaged = false;
        }
        yield return new WaitForSeconds(delay);
        //turn on fire
        isOn = true;



    }
}
