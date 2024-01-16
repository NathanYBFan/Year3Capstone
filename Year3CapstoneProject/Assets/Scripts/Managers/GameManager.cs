using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton Initialization
    public static GameManager _Instance;

    // Public Variables
    public bool inGame;
    public bool isPaused;

    // Serialize Fields
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Pause Menu GameObject")]
    private GameObject pauseMenu;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("All players as a referenceable gameobject")]
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
        BulletObjectPoolManager._Instance.ResetAllBullets();
        // Start Player stuff
    }

    public void PauseGame()
    {
        if (!inGame) return; // TODO NATHANF: If not in game should have different use
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);

        if (isPaused)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }
}
