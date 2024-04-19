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

    [SerializeField]
    private string CFname;

    private float[] holdSpeeds;

    [SerializeField]
    private GameObject[] compatibleCFs;
    public GameObject[] CompatibleCFs { get { return compatibleCFs; } }
    public string Name { get { return CFname; } }
    public float Timer { get { return timer; } }


    private int kickSpeed = 7;

    private float kickTime = 0.5f;

    private bool kicked = false;

    // Start is called before the first frame update
    void Start()
    {
        ChaosFactorManager._Instance.activeCFCount++;
        holdSpeeds = new float[GameManager._Instance.Players.Count];
        //get random player from the gamemanager player list, called target player
        for (int i = 0; i < GameManager._Instance.Players.Count; i++)
        {
            holdSpeeds[i] = GameManager._Instance.Players[i].GetComponent<PlayerStats>().MovementSpeed;
            GameManager._Instance.Players[i].GetComponent<PlayerStats>().MovementSpeed = playerSpeed;
            GameManager._Instance.Players[i].GetComponent<PlayerBody>().BootCF = true;
        }




    }

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    public void Kick(GameObject kicker)
    {
        KickSound(kicker);
        foreach (GameObject p in GameManager._Instance.Players)
        {
            if (p != kicker && p != null) 
            { 


            //check distance
            Vector3 distance = p.transform.position - kicker.transform.position; //not problem

            //GET MAGNITUDE OF DISTANCE
            float dist = distance.magnitude; // not problem


                // this is probably the problem
            //get direction
            Vector3 localDir = Quaternion.Inverse(kicker.GetComponent<PlayerBody>().Pivot.transform.rotation) * (p.transform.position - kicker.transform.position);


            if (dist < bootRange && localDir.z > 0 && !kicked)
            {

                kicker.GetComponent<PlayerBody>().LegPivot.transform.rotation = kicker.GetComponent<PlayerBody>().Pivot.transform.rotation;
                Vector3 kickDirect = new Vector3(distance.x, 1.3f, distance.z);
                StartCoroutine(Kick(kickDirect, p.GetComponent<Rigidbody>()));
            }
        }
        }
    }

    private IEnumerator Kick(Vector3 kickDir, Rigidbody rb)
    {
        kicked = true;
        float elapsedTime = 0f;
        rb.gameObject.GetComponent<PlayerStats>().Booted = true;
        while (elapsedTime < kickTime)
        {
            float distThisFrame = kickSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + kickDir * distThisFrame);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        kicked = false;

        yield return new WaitForSeconds(0.30f);
        rb.gameObject.GetComponent<PlayerStats>().Booted = false;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < GameManager._Instance.Players.Count; i++)
        {
            if (GameManager._Instance.Players[i] == null) 
            {

                GameManager._Instance.Players[i].GetComponent<PlayerStats>().MovementSpeed = holdSpeeds[i];
                GameManager._Instance.Players[i].GetComponent<PlayerStats>().CanShoot = true;
                GameManager._Instance.Players[i].GetComponent<PlayerBody>().BootCF = false;
            }

        }
        ChaosFactorManager._Instance.activeCFCount--;
    }

	public void OnEndOfChaosFactor(bool earlyEnd)
	{
        Destroy(gameObject);
	}

    //Plays the kick impact sound
    private void KickSound(GameObject kicker)
    {
        float randPitch = Random.Range(0.8f, 1.5f);
        AudioSource audioSource = AudioManager._Instance.ChooseEnvAudioSource();
        if (audioSource != null)
        {
            audioSource.pitch = randPitch;
            AudioManager._Instance.PlaySoundFX(AudioManager._Instance.CFAudioList[8], audioSource);
        }

    }

}
