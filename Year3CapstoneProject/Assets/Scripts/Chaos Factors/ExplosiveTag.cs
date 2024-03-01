using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class ExplosiveTag : MonoBehaviour
{
    [SerializeField]
    private GameObject UnSpawnedbelt;

    [SerializeField]
    private int beltRotSpeed;

    [SerializeField]
    private int damage;

    [SerializeField]
    private int playerSpeed;


    private float[] holdSpeeds;
    private GameObject Spawnedbelt;
    private GameObject targetPlayer;
    



    // Start is called before the first frame update
    void Start()
    {

        holdSpeeds = new float[GameManager._Instance.Players.Count];
        //get random player from the gamemanager player list, called target player
        for (int i = 0; i < GameManager._Instance.Players.Count; i++)
        {
            holdSpeeds[i] = GameManager._Instance.Players[i].GetComponent<PlayerStats>().MovementSpeed;
            GameManager._Instance.Players[i].GetComponent<PlayerStats>().MovementSpeed = playerSpeed;
        }

        int random = Random.Range(0,3);

        targetPlayer = GameManager._Instance.Players[0];

        //spawn bomb belt on player
        Spawnedbelt = Instantiate(UnSpawnedbelt, new Vector3 (targetPlayer.transform.position.x, targetPlayer.transform.position.y+2, targetPlayer.transform.position.z), Quaternion.Euler(new Vector3(-90, 0, 0)), this.transform);
    }

    // Update is called once per frame
    void Update()
    {

        //move with target player
        Spawnedbelt.transform.position = new Vector3(targetPlayer.transform.position.x, targetPlayer.transform.position.y + 2, targetPlayer.transform.position.z);
        Spawnedbelt.transform.Rotate(new Vector3(0, 0, beltRotSpeed) * Time.deltaTime);
        //if collide with player, swap to that player, and play and effect

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
      
        }
        targetPlayer.GetComponent<PlayerStats>().TakeDamage(damage);
    }





}
