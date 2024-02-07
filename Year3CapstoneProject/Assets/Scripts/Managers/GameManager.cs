using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class GameManager : MonoBehaviour
{
	// Singleton Initialization
	public static GameManager _Instance;

    #region SerializeFields
    [SerializeField]
	[Foldout("Dependencies"), Tooltip("List of all platforms")]
    private List<GameObject> platforms;

    [SerializeField]
	[Foldout("Dependencies"), Tooltip("Pause Menu GameObject")]
	private GameObject pauseMenu;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Object Holding the playerInputManager script")]
    private GameObject playerInputManager;

    [SerializeField]
	[Foldout("Dependencies"), Tooltip("All players as a referenceable gameobject")]
	private List<GameObject> players;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("All modifiers accessible in the game.")]
	private List<Modifier> modifiers;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("array of spawnpoints")]
    private Transform[] stageSpawnPoints; // TODO NATHANF: ADD SPAWNPOINTS ON STAGE INSTANTIATE

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("array of spawnpoints")]
    private InputSystemUIInputModule uiInputModule;

    [SerializeField, ReadOnly]
	[Foldout("Stats"), Tooltip("Selectected game mode to load")]
	private string selectedGameMode;
    #endregion

    // Public Variables
    private bool inGame;
    private bool isPaused;

    #region Getters&Setters
    public List<GameObject> Players { get { return players; } set { players = value; } }
    public List<GameObject> Platforms { get { return platforms; } set { platforms = value; } }
    public List<Modifier> Modifiers { get { return modifiers; } }
	public string SelectedGameMode { get { return selectedGameMode; } set { selectedGameMode = value; } }
    public Transform[] StageSpawnPoints { get { return stageSpawnPoints; } set { stageSpawnPoints = value; } }
	public InputSystemUIInputModule UiInputModule { get { return uiInputModule; } }
	public bool InGame { get { return inGame; } set { inGame = value; } }
	public bool IsPaused { get { return isPaused; } set { isPaused = value; } }
    #endregion

    private void Awake()
	{
		if (_Instance != null && _Instance != this)
		{
			Debug.LogWarning("Destroyed a repeated GameManager");
			Destroy(this.gameObject);
		}

		else if (_Instance == null)
			_Instance = this;
		platforms = new List<GameObject>();
	}

	// Play game initial setups
	public void StartNewGame()
	{
		ChaosFactorManager._Instance.Reset();
		BulletObjectPoolManager._Instance.ResetAllBullets();
		playerInputManager.SetActive(true);

		SpawnPlayersAtSpawnpoint();

		// Start Player stuff
	}

	// Reset everything when game ends
	public void EndGame()
	{
        ChaosFactorManager._Instance.Reset();
        BulletObjectPoolManager._Instance.ResetAllBullets();
		playerInputManager.SetActive(false);

		ResetPlayersToVoid();
    }

    public void PauseGame()
	{
		if (!inGame) return;

		isPaused = !isPaused;
		pauseMenu.SetActive(isPaused);

		if (isPaused)
			Time.timeScale = 0f;
		else
			Time.timeScale = 1f;
	}

	public void WinConditionMet() // TODO NATHANF: FILL OUT
	{

	}

	// Method to reset everything when quitting to main menu
	public void QuitToMainMenu() // TODO NATHANF: INCORPORATE THIS INTO ENDING THE GAME
	{
        PauseGame();
		inGame = false;

		foreach (GameObject player in Players)
		{
			player.GetComponent<Rigidbody>().velocity = Vector3.zero;
			player.transform.position = new Vector3(-100, 0, 0);
			player.GetComponent<PlayerStats>().ResetPlayer();
		}
	}

	/// <summary>
	/// This is to give functionality to the "Give" command for the Command Prompt menu. (Debug purposes)
	/// </summary>
	/// <param name="playerIndex">The index of the player to give some modifier to.</param>
	/// <param name="modifierName">The name of the modifier being given.</param>
	public void CommandGive(int playerIndex, string modifierName) // There is probably a better way to do this
	{
		//Converting command modifier name to actual modifier name.
		switch (modifierName)
		{
			case "overheated":
				modifierName = "Overheated";
				break;
			case "overcharged":
				modifierName = "Overcharged";
				break;
			case "power-savingmode":
				modifierName = "Power-Saving Mode";
				break;
			case "homingbullet":
				modifierName = "Homing Bullet";
				break;
			case "fragmentation":
				modifierName = "Fragmentation";
				break;
			case "trifecta":
				modifierName = "Trifecta";
				break;
			case "unstableblast":
				modifierName = "Unstable Blast";
				break;
			case "ygdwm":
				modifierName = "You're Going Down With Me!";
				break;
			default:
				Debug.LogWarning("Invalid modifier entry!");
				return;
		}
		Modifier modifierToGive = modifiers.Find(m => m.modifierName == modifierName); //Search for the modifier to give within the list of existing modifiers.

		//If it exists...
		if (modifierToGive != null)
		{
			//Give to all players
			if (playerIndex == -1)
			{
				foreach (GameObject p in players)
				{
					modifierToGive.AddEffects(p.GetComponent<PlayerStats>());
					p.GetComponent<PlayerStats>().modifiers.Add(modifierToGive);
				}
			}
			//Give to a specific player
			else
			{
				GameObject playerToModify = players.Find(p => p.GetComponent<PlayerBody>().PlayerIndex == playerIndex);
				//If the provided player number matches a player.
				if (playerToModify != null)
				{
					modifierToGive.AddEffects(playerToModify.GetComponent<PlayerStats>());
					playerToModify.GetComponent<PlayerStats>().modifiers.Add(modifierToGive);
				}
				//There's no player with this number.
				else Debug.LogWarning("Player of index " + playerIndex + " doesn't exist!");

			}
		}
	}

	// Spawn players at appropiate spawn points
	private void SpawnPlayersAtSpawnpoint() // TODO NATHANF: INCORPORATE THIS INTO GAME START PROCEDURE
	{
		foreach(GameObject player in players)
		{
			player.transform.position = stageSpawnPoints[player.GetComponent<PlayerBody>().PlayerIndex].position;
		}
	}

	// Reset player to platform on end game
	private void ResetPlayersToVoid() // TODO NATHANF: INCORPORATE THIS INTO GAME END PROCEDURE
	{
		foreach(GameObject player in players)
		{
			player.transform.position = new Vector3(-100, 0, 0);
		}
	}
}
