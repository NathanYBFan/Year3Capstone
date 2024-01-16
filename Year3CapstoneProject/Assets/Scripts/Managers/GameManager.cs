using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton Initialization
    public static GameManager _Instance;

    public bool inGame;

    // Serialize Fields
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Player 1 referenceable gameobject")]
    private List<GameObject> players;

    // Getters
    public List<GameObject> Players { get { return players; } set { players = value; } }

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

    // Play game initial setups
    public void StartNewGame()
    {
        ChaosFactorManager._Instance.Reset();
        // Start Player stuff
        // Setup Bullet Instances? <-- Could be done at launch, reset them
    }


}
