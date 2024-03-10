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

    private int kickSpeed = 7;

    private float kickTime = 0.5f;

    private bool kicked = false;

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

        foreach (GameObject p in GameManager._Instance.Players)
        {
            if (p == kicker || p == null) { break; }
            //check distance
            Vector3 distance = p.transform.position - kicker.transform.position;

            //GET MAGNITUDE OF DISTANCE
            float dist = distance.magnitude;

            //get direction
            Vector3 localDir = Quaternion.Inverse(kicker.transform.rotation) * (p.transform.position - kicker.transform.position);
            
            if (dist < 5 && localDir.z > 0 && !kicked)
            {
                print(p.name);
                Vector3 kickDirect = new Vector3(distance.x, 1.3f, distance.z);
                StartCoroutine(Kick(kickDirect, p.GetComponent<Rigidbody>()));
            }
        }
    }

    private IEnumerator Kick(Vector3 kickDir, Rigidbody rb)
    {
        kicked = true;
        float elapsedTime = 0f;
        while (elapsedTime < kickTime)
        {
            float distThisFrame = kickSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + kickDir * distThisFrame);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        kicked = false;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < GameManager._Instance.Players.Count; i++)
        {
            GameManager._Instance.Players[i].GetComponent<PlayerStats>().MovementSpeed = holdSpeeds[i];
            GameManager._Instance.Players[i].GetComponent<PlayerStats>().CanShoot = true;
            
            GameManager._Instance.Players[i].GetComponent<PlayerBody>().BootCF = false;
        }
    }
}