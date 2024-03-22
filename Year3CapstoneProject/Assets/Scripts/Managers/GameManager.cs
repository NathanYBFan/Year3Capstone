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
	[Foldout("Dependencies"), Tooltip("List of all HUD elements")]
	private List<GameObject> hudBars;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("Pause Menu GameObject")]
	private GameObject pauseMenu;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("Pause Menu GameObject")]
	private CenterCamera cam;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("All players as a referenceable gameobject")]
	private List<GameObject> players;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("array of spawnpoints")]
	private List<Transform> stageSpawnPoints;

	[SerializeField, ReadOnly]
	[Foldout("Stats"), Tooltip("List of players who are dead")]
	private List<GameObject> deadPlayerList;

	[SerializeField, ReadOnly]
	[Foldout("Stats"), Tooltip("List of players who are dead")]
	private int currentRound = 0;

	[SerializeField]
	[Foldout("Stats"), Tooltip("List of players who are dead")]
	private int maxRounds;
	#endregion

	#region PrivateVariables
	private bool inGame;
	private bool isPaused;
	private newLevelBuilder levelBuilder;
	private int playerWinnerIndex = -1;
	private bool inPauseMenu = false, inSettingsMenu = false;
	#endregion

	#region Getters&Setters
	public List<GameObject> Players { get { return players; } set { players = value; } }
	public List<GameObject> Platforms { get { return platforms; } set { platforms = value; } }
	public List<Transform> StageSpawnPoints { get { return stageSpawnPoints; } set { stageSpawnPoints = value; } }
	public bool InGame { get { return inGame; } set { inGame = value; } }
	public bool IsPaused { get { return isPaused; } set { isPaused = value; } }
	public int CurrentRound { get { return currentRound; } set { currentRound = value; } }
	public int MaxRounds { get { return maxRounds; } set { maxRounds = value; } }
	public newLevelBuilder LevelBuilder { get { return levelBuilder; } set { levelBuilder = value; } }
	public int PlayerWinnerIndex { get { return playerWinnerIndex; } }
	public bool InPauseMenu { get { return inPauseMenu; } set { inPauseMenu = value; } }
	public bool InSettingsMenu { get { return inSettingsMenu; } set { inSettingsMenu = value; } }
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
		RemoveStage();

		foreach (GameObject player in players)
			player.GetComponent<PlayerStats>().ResetPlayer();

		if (levelBuilder != null)
		{
			levelBuilder.buildLevel(currentRound % 3);
			SpawnPlayersAtSpawnpoint();
			cam.Center();
			ChaosFactorManager._Instance.ChaosFactorActive = false;
		}

		ChaosFactorManager._Instance.Reset();
		ChaosFactorManager._Instance.StartChaosFactor();
		BulletObjectPoolManager._Instance.ResetAllBullets();

		// Clear dead player list
		deadPlayerList.Clear();

		// Initialize Players
		foreach (GameObject player in players)
		{
			player.SetActive(true);
			player.GetComponentInChildren<PlayerStats>().IsDead = false;
			player.GetComponentInChildren<PlayerBody>().Reset = false;
			player.GetComponentInChildren<PlayerStats>().ResetMaterialEmissionColor();
		}
		// Enable Player HUD's
		foreach (GameObject h in hudBars)
		{
            h.SetActive(true);
			h.GetComponent<Bars>().FullReset();
        }

        inPauseMenu = false;
		inSettingsMenu = false;
    }

	public void PlayerDied(GameObject playerThatDied)
	{
		deadPlayerList.Add(playerThatDied);
		ResetPlayerToVoid(playerThatDied);

		if (deadPlayerList.Count < players.Count - 1) return;

		EndRound();
	}

	public void PauseGame(bool enablePauseMenu)
	{
		if (!inGame) return;

		isPaused = !isPaused;
		if (enablePauseMenu)
			pauseMenu.SetActive(isPaused);

		if (isPaused)
			Time.timeScale = 0f;
		else
			Time.timeScale = 1f;
	}

	private void EndRound()
	{
		// Reset --------
		foreach (GameObject h in hudBars)
			h.SetActive(false);

		BulletObjectPoolManager._Instance.ResetAllBullets();

		// Make sure all players are in the list
		for (int i = 0; i < players.Count; i++)
		{
			if (!deadPlayerList.Contains(players[i]))
				deadPlayerList.Add(players[i]);
		}
		// Remove players from stage
		ResetPlayersToVoid();

		// Incriment round counter
		currentRound++;

		// Assign points
		PlayerStatsManager._Instance.IncreasePoints(deadPlayerList[0].GetComponent<PlayerBody>().PlayerIndex, PlayerStatsManager._Instance.PointsToGiveForPosition[3]); // First to die,	least points
		PlayerStatsManager._Instance.IncreasePoints(deadPlayerList[1].GetComponent<PlayerBody>().PlayerIndex, PlayerStatsManager._Instance.PointsToGiveForPosition[2]); // Second to die,	some points
		PlayerStatsManager._Instance.IncreasePoints(deadPlayerList[2].GetComponent<PlayerBody>().PlayerIndex, PlayerStatsManager._Instance.PointsToGiveForPosition[1]); // Third to die,	more points
		PlayerStatsManager._Instance.IncreasePoints(deadPlayerList[3].GetComponent<PlayerBody>().PlayerIndex, PlayerStatsManager._Instance.PointsToGiveForPosition[0]); // Last one alive, most points

		if (currentRound >= MaxRounds)
		{
			WinConditionMet();
			return;
		}

		// Bring up modifier Menu;
		ModifierManager._Instance.PlayerToModify = deadPlayerList[0]; // First dead should be modified
		ModifierManager._Instance.OpenModifierMenu(deadPlayerList[0].GetComponent<PlayerBody>().PlayerIndex); // Open modifier menu for dead player
	}

	public void WinConditionMet()
	{
		// Make local variables
		List<int> playerWinOrder = new List<int>();     // Saved win order
		List<int> localPoints = new List<int>();        // Local save of the points

		// Fill local variables
		for (int i = 0; i < PlayerStatsManager._Instance.PlayerPoints.Length; i++)
			localPoints.Add(PlayerStatsManager._Instance.PlayerPoints[i]);

		// For the number of players there are
		for (int j = 0; j < players.Count; j++)
		{
			int max = 0;    // Max saved number
			int index = 0;  // Index max number is found

			for (int i = 0; i < localPoints.Count; i++) // Check the points list
			{
				if (localPoints[i] > max)
				{
					max = localPoints[i]; // Get max number
					index = i;
				}
			}

			localPoints[index] = -1;
			playerWinOrder.Add(index); // Most points to smallest
		}
		playerWinnerIndex = playerWinOrder[0];
		EndGame();
		LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[7], true);
	}

	// Reset everything when game ends
	public void EndGame()
	{
		foreach (GameObject h in hudBars)
			h.SetActive(false);
        foreach (GameObject player in players)
            player.GetComponent<PlayerStats>().FullResetPlayer();
        pauseMenu.SetActive(false);
		ChaosFactorManager._Instance.Reset();
		BulletObjectPoolManager._Instance.ResetAllBullets();
		PlayerStatsManager._Instance.ResetStats();
		ModifierManager._Instance.CloseAllMenus();
		currentRound = 0;
        inPauseMenu = false;
        inSettingsMenu = false;
        QuitToMainMenu();
	}

	// Method to reset everything when quitting to main menu
	private void QuitToMainMenu()
	{
		if (Time.timeScale == 0)
			PauseGame(false);
		inGame = false;
		AudioManager._Instance.PlayMusic(0);
		ResetPlayersToVoid();
		RemovePlayerModels();
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
		foreach (GameObject player in players)
		{
			player.GetComponent<Rigidbody>().velocity = Vector3.zero;
			player.transform.position = stageSpawnPoints[player.GetComponent<PlayerBody>().PlayerIndex].position;
			player.GetComponentInChildren<CapsuleCollider>().enabled = true;
			player.GetComponentInChildren<Rigidbody>().useGravity = true;
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
			player.GetComponentInChildren<CapsuleCollider>().enabled = true;
			player.GetComponent<PlayerBody>().SoftReset();
		}
	}

	private void ResetPlayerToVoid(GameObject player)
	{
		player.GetComponent<Rigidbody>().velocity = Vector3.zero;
		player.GetComponentInChildren<CapsuleCollider>().enabled = true;
		player.transform.position = new Vector3(-100, 0, 0);
		player.GetComponentInChildren<Rigidbody>().useGravity = false;
		player.GetComponent<PlayerBody>().SoftReset();
	}

	// Remove Player Models
	private void RemovePlayerModels()
	{
		foreach (GameObject player in Players)
			player.GetComponent<PlayerStats>().FullResetPlayer(); // Remove player model
	}

	public void RemoveStage()
	{
		for (int i = 0; i < platforms.Count; i++)
			Destroy(platforms[i]);
		platforms.Clear();
		stageSpawnPoints.Clear();
	}
}
