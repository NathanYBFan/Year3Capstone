using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField, Foldout("Player Stats"), ReadOnly]
    private int jumpHeight = 15;

    [SerializeField, Foldout("Player Stats")]
    private int slideLength = 6;

    [SerializeField, Foldout("Player Stats")]
    private float yippeeee = 15;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
