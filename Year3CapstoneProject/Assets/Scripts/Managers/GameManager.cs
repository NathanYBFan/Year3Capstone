using NaughtyAttributes;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Rendering;

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
    private Transform[] stageSpawnPoints; // TODO NATHANF: ADD SPAWNPOINTS ON INSTANTIATE

    [SerializeField, ReadOnly]
	[Foldout("Stats"), Tooltip("Selectected game mode to load")]
	private string selectedGameMode;

	// Getters
	public List<GameObject> Players { get { return players; } set { players = value; } }
	public List<Modifier> Modifiers { get { return modifiers; } }
	public string SelectedGameMode { get { return selectedGameMode; } set { selectedGameMode = value; } }
    public Transform[] StageSpawnPoints { get { return stageSpawnPoints; } set { stageSpawnPoints = value; } }

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
		playerInputManager.SetActive(true);

		SpawnPlayersAtSpawnpoint();

		// Start Player stuff
	}

	public void EndGame()
	{
        ChaosFactorManager._Instance.Reset();
        BulletObjectPoolManager._Instance.ResetAllBullets();
		playerInputManager.SetActive(false);

		ResetPlayersToVoid();
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

	public void WinConditionMet()
	{

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

	private void SpawnPlayersAtSpawnpoint()
	{
		foreach(GameObject player in players)
		{
			player.transform.position = stageSpawnPoints[player.GetComponent<PlayerBody>().PlayerIndex].position;
		}
	}

	private void ResetPlayersToVoid()
	{
		foreach(GameObject player in players)
		{
			player.transform.position = new Vector3(-100, 0, 0);
		}
	}







}
