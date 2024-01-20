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

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("All modifiers accessible in the game.")]
	private List<Modifier> modifiers;

	[SerializeField, ReadOnly]
	[Foldout("Stats"), Tooltip("The stages possible")]
	private string[] stageOfPlay = { "Menus", "In Play", "Modifier", "Win Screen" };

	[SerializeField, ReadOnly]
	[Foldout("Stats"), Tooltip("Selectected game mode to load")]
	private string selectedGameMode;

	[SerializeField, ReadOnly]
	[Foldout("Stats"), Tooltip("Stage the game is currently at")]
	private string stageAt;

	// Getters
	public List<GameObject> Players { get { return players; } set { players = value; } }

	public List<Modifier> Modifiers { get { return modifiers; } }
	public string SelectedGameMode { get { return selectedGameMode; } set { selectedGameMode = value; } }
	public string StageAt { get { return stageAt; } set { stageAt = value; } }

	private void Start()
	{
		stageAt = stageOfPlay[0];
	}

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

	public void WinConditionMet()
	{

	}

	public void CommandGive(int playerIndex, string modifierName)
	{
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
			default:
				Debug.LogWarning("Invalid modifier entry!");
				return;

		}
		Modifier modifierToGive = modifiers.Find(m => m.modifierName == modifierName);
		if (modifierToGive != null)
		{
			if (playerIndex == -1)
			{
				foreach (GameObject p in players)
				{
					modifierToGive.AddEffects(p.GetComponent<PlayerStats>());
					p.GetComponent<PlayerStats>().modifiers.Add(modifierToGive);
				}
			}
			else
			{
				GameObject playerToModify = players.Find(p => p.GetComponent<PlayerBody>().PlayerIndex == playerIndex);
				if (playerToModify != null)
				{
					modifierToGive.AddEffects(playerToModify.GetComponent<PlayerStats>());
					playerToModify.GetComponent<PlayerStats>().modifiers.Add(modifierToGive);
				}
				else Debug.LogWarning("Player of index " + playerIndex + " doesn't exist!");
				
			}
		}
	}
}
