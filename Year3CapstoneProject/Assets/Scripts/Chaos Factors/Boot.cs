using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boot : MonoBehaviour, ChaosFactor
{



    [SerializeField]
    private int playerSpeed;
    [SerializeField]
    private float bootRange;

    [SerializeField]
    private float timer;

    private float[] holdSpeeds;
    public float Timer { get { return timer; } }


    // Start is called before the first frame update
    void Start()
    {
        holdSpeeds = new float[GameManager._Instance.Players.Count];
        //get random player from the gamemanager player list, called target player
        for (int i = 0; i < GameManager._Instance.Players.Count; i++)
        {
            holdSpeeds[i] = GameManager._Instance.Players[i].GetComponent<PlayerStats>().MovementSpeed;
            GameManager._Instance.Players[i].GetComponent<PlayerStats>().MovementSpeed = playerSpeed;
            GameManager._Instance.Players[i].GetComponent<PlayerBody>().BootCF = true;
        }




    }


    // Update is called once per frame
    //void Update()
    //{

    //}


    //take player transform
    public void Kick(GameObject kicker)
    {

        //Determin if player is in fron of you

        //check distance

        foreach (GameObject p in GameManager._Instance.Players)
        { 
            if (p != kicker && p != null)
            {
                //gt player position

                //check distance

                Vector3 distance = p.transform.position - kicker.transform.position;

                float dist = distance.magnitude;

                
                //print(p.name+": " + distance);


                if (dist < 5)
                {
                    Vector3 localDir = Quaternion.Inverse(kicker.transform.rotation) * (p.transform.position - kicker.transform.position);


                    if (localDir.z > 0)
                    {
                        print(p.name);
                        p.GetComponent<Rigidbody>().AddForce((new Vector3(distance.x*10, 2, distance.z*10) * 15f), ForceMode.VelocityChange);

                    }

                }


            }
        }


        //Add force to them




    }

    private void OnDestroy()
    {
        for (int i = 0; i < GameManager._Instance.Players.Count; i++)
        {
            GameManager._Instance.Players[i].GetComponent<PlayerStats>().MovementSpeed = holdSpeeds[i];
            GameManager._Instance.Players[i].GetComponent<PlayerBody>().CanShoot = true;
            
            GameManager._Instance.Players[i].GetComponent<PlayerBody>().BootCF = false;
        }
        
    }
}
