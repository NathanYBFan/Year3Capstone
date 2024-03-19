using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class ExplosiveTag : MonoBehaviour, ChaosFactor
{
    [SerializeField]
    private GameObject UnSpawnedbelt;

    [SerializeField]
    private int beltRotSpeed;

    [SerializeField]
    private int damage;

    [SerializeField]
    private int playerSpeed;

    [SerializeField]
    private float timer;

    private float[] holdSpeeds;
    private GameObject Spawnedbelt;
    private GameObject targetPlayer;

    public float Timer { get { return timer; } }


    private void OnEnable()
    {
        holdSpeeds = new float[GameManager._Instance.Players.Count];
        //get random player from the gamemanager player list, called target player
        for (int i = 0; i < GameManager._Instance.Players.Count; i++)
        {
            holdSpeeds[i] = GameManager._Instance.Players[i].GetComponent<PlayerStats>().MovementSpeed;
            GameManager._Instance.Players[i].GetComponent<PlayerStats>().MovementSpeed = playerSpeed;
            GameManager._Instance.Players[i].GetComponent<PlayerStats>().ChaosFactorCanShoot = false;
        }

        bool loop = true;
        while (loop)
        {
            int random = Random.Range(0, 3);

            targetPlayer = GameManager._Instance.Players[random];

            if (targetPlayer.GetComponent<PlayerStats>().IsDead == true) { print("Dead player selected, trying agian"); }

            else
            {
                loop = false;
            }


        }



        //spawn bomb belt on player
        Spawnedbelt = Instantiate(UnSpawnedbelt, new Vector3(targetPlayer.transform.position.x, targetPlayer.transform.position.y + 2, targetPlayer.transform.position.z), Quaternion.Euler(new Vector3(-90, 0, 0)), this.transform);
    }


    // Update is called once per frame
    void Update()
    {

        //move with target player
        Spawnedbelt.transform.position = new Vector3(targetPlayer.transform.position.x, targetPlayer.transform.position.y + 2, targetPlayer.transform.position.z);
        Spawnedbelt.transform.Rotate(new Vector3(0, 0, beltRotSpeed) * Time.deltaTime);

        if (targetPlayer.GetComponent<PlayerStats>().IsDead == true)
        {
            Destroy(gameObject);
        }

    }


    public void swapTarget(GameObject p)
    {
        targetPlayer = p;
    }



    private void OnDestroy()
    {
        for (int i = 0; i < GameManager._Instance.Players.Count; i++)
        {
            GameManager._Instance.Players[i].GetComponent<PlayerStats>().MovementSpeed = holdSpeeds[i];
            GameManager._Instance.Players[i].GetComponent<PlayerStats>().CanShoot = true;
            GameManager._Instance.Players[i].GetComponent<PlayerStats>().ChaosFactorCanShoot = true;
        }
        targetPlayer.GetComponent<PlayerStats>().TakeDamage(damage, DamageType.ChaosFactor);
        GameObject.Find("VCam").GetComponent<CameraShake>().ShakeCamera(1, 0.5f);
    }
}
