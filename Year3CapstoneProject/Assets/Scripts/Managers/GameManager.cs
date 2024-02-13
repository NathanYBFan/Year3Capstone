using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	[Foldout("Dependencies"), Tooltip("All players as a referenceable gameobject")]
	private List<GameObject> players;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("array of spawnpoints")]
    private Transform[] stageSpawnPoints; // TODO NATHANF: ADD SPAWNPOINTS ON STAGE INSTANTIATE

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("array of spawnpoints")]
    private InputSystemUIInputModule uiInputModule;

    [SerializeField, ReadOnly]
	[Foldout("Stats"), Tooltip("Selectected game mode to load")]
	private string selectedGameMode;

    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("List of players who are dead")]
    private List<GameObject> deadPlayerList;
    #endregion

    #region PrivateVariables
    private bool inGame;
    private bool isPaused;
    #endregion

    #region Getters&Setters
    public List<GameObject> Players { get { return players; } set { players = value; } }
    public List<GameObject> Platforms { get { return platforms; } set { platforms = value; } }
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
		ChaosFactorManager._Instance.StartChaosFactor();
		BulletObjectPoolManager._Instance.ResetAllBullets();

		SpawnPlayersAtSpawnpoint();

		// Start Player stuff
	}

	// Reset everything when game ends
	public void EndGame()
	{
        ChaosFactorManager._Instance.Reset();
        BulletObjectPoolManager._Instance.ResetAllBullets();
		QuitToMainMenu();
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

	public void WinConditionMet(List<int> playerWinOrder) // TODO NATHANF: FILL OUT
	{
		// go to end screen
		// reset players
		EndGame();
	}

	// Method to reset everything when quitting to main menu
	private void QuitToMainMenu()
	{
        PauseGame();
		inGame = false;
		ResetPlayersToVoid();
		RemovePlayerModels();
    }

    public void PlayerDied(GameObject playerThatDied)
    {
        deadPlayerList.Add(playerThatDied);

        if (deadPlayerList.Count >= players.Count - 1)
            EndRound();
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
		Modifier modifierToGive = ModifierManager._Instance.ListOfModifiers.Find(m => m.modifierName == modifierName); //Search for the modifier to give within the list of existing modifiers.

		//If it exists...
		if (modifierToGive != null)
		{
			//Give to all players
			if (playerIndex == -1)
			{
				foreach (GameObject p in players)
				{
					modifierToGive.AddEffects(p.GetComponent<PlayerStats>());
					p.GetComponent<PlayerStats>().ModifiersOnPlayer.Add(modifierToGive);
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
					playerToModify.GetComponent<PlayerStats>().ModifiersOnPlayer.Add(modifierToGive);
				}
				//There's no player with this number.
				else Debug.LogWarning("Player of index " + playerIndex + " doesn't exist!");
			}
		}
	}

	// Spawn players at appropiate spawn points
	private void SpawnPlayersAtSpawnpoint()
	{
		foreach(GameObject player in players)
		{
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
			player.transform.position = stageSpawnPoints[player.GetComponent<PlayerBody>().PlayerIndex].position;
			player.SetActive(true);
        }
    }

	// Reset player to platform on end game
	private void ResetPlayersToVoid()
	{
        foreach (GameObject player in Players)
        {
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.transform.position = new Vector3(-100, 0, 0);
        }
    }

	private void RemovePlayerModels()
	{
		foreach(GameObject player in Players)
            player.GetComponent<PlayerStats>().ResetPlayer(); // Remove player model
    }

	private void EndRound()
	{
		// Make sure all players are in the list
		for (int i = 0; i < players.Count; i++)
        {
			if (!deadPlayerList.Contains(players[i]))
				deadPlayerList.Add(players[i]);
		}
		// Remove players from stage
		ResetPlayersToVoid();

		// Bring up modifier Menu;
		ModifierManager._Instance.PlayerToModify = deadPlayerList[0]; // First dead should be modified
		ModifierManager._Instance.OpenModifierMenu(); // Open modifier menu for dead player

		// Assign points
		PlayerStatsManager._Instance.IncreasePoints(0, PlayerStatsManager._Instance.PointsToGiveForPosition[3]); // First to die,	least points
        PlayerStatsManager._Instance.IncreasePoints(1, PlayerStatsManager._Instance.PointsToGiveForPosition[2]); // Second to die,	some points
        PlayerStatsManager._Instance.IncreasePoints(2, PlayerStatsManager._Instance.PointsToGiveForPosition[1]); // Third to die,	more points
        PlayerStatsManager._Instance.IncreasePoints(3, PlayerStatsManager._Instance.PointsToGiveForPosition[0]); // Last one alive, most points

        // TODO NATHANF: Reset stage
    }
}
