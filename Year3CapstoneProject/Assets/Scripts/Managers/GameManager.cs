using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Player 1 referenceable gameobject")]
    private GameObject player1;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("P")]
    private GameObject player2;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("P")]
    private GameObject player3;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("P")]
    private GameObject player4;

    public GameObject Player2 { get { return player2; } }
    public GameObject Player3 { get { return player3; } }
    public GameObject Player4 { get { return player4; } }


    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Debug.LogWarning("Destroyed a repeated GameManager");
            Destroy(this.gameObject);
        }

        else if (_Instance == null)
            _Instance = this;
    }



}
