using NaughtyAttributes;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton Initialization
    public static GameManager _Instance;

    public bool inGame;

    // Serialize Fields
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

    // Getters
    public GameObject Player1 { get { return player1; } }
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

    // Play game initial setups
    public void StartNewGame()
    {
        // Start Chaos Factor
        // Start Player stuff
        // Setup Bullet Instances? <-- Could be done at launch, reset them
    }
}
