using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelectMenu : MonoBehaviour
{
	#region SerializeFields
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private GameObject[] characterSelectMenus = new GameObject[4];

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private InputSystemUIInputModule[] uiInputModules = new InputSystemUIInputModule[4];

	[SerializeField]
	[Foldout("Stats"), Tooltip("Character to instantiate")]
	private CharacterStatsSO[] characterSelectedByPlayers = new CharacterStatsSO[4];

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("List of all possible colors to choose from")]
	private Color[] listOfColors;
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("List of all possible textures to choose from (Link to colors)")]
	private Texture2D[] listOfTextures;
	[SerializeField]
	[Foldout("Stats"), Tooltip("Color to assign to players")]
	private Texture[] colorSelectedByPlayers = new Texture[4];

	[SerializeField]
	[Foldout("Stats"), Tooltip("Color to assign to players")]
	private Color[] uiColorSelectedByPlayers = new Color[4];
	#endregion

	public CharacterStatsSO[] CharacterSelectedByPlayers { get { return characterSelectedByPlayers; } set { characterSelectedByPlayers = value; } }
	public Texture[] ColorSelectedByPlayers { get { return colorSelectedByPlayers; } set { colorSelectedByPlayers = value; } }
	public Texture2D[] ListOfTextures { get { return listOfTextures; } set { listOfTextures = value; } }
	public Color[] UIColorSelectedByPlayers { get { return uiColorSelectedByPlayers; } set { uiColorSelectedByPlayers = value; } }
	public Color[] ListOfColors { get { return listOfColors; } set { listOfColors = value; } }
	public GameObject[] CharacterSelectMenus { get { return characterSelectMenus; } set { characterSelectMenus = value; } }
	List<Color> availableColors = new List<Color>();
	List<Texture2D> availableTextures = new List<Texture2D>();
	private void OnEnable()
	{
		MenuInputManager._Instance.InCharacterSelect = true;
		MenuInputManager._Instance.CharacterSelectMenu = this;
		for (int i = 0; i < MenuInputManager._Instance.PlayerInputs.Count; i++)
			NewPlayerInputJoined(i);
		MenuInputManager._Instance.EnterCharacterSelectScreen();

		for (int i = 0; i < uiColorSelectedByPlayers.Length; i++)
		{
			uiColorSelectedByPlayers[i] = Color.black;
			colorSelectedByPlayers[i] = null;
		}
	}
	private void Start()
	{
		UpdateColorOptions();
	}
	private void OnDisable()
	{
		MenuInputManager._Instance.ExitCharacterSelectScreen();
		MenuInputManager._Instance.InCharacterSelect = false;
	}

	private void Update() // Debug
	{
		if (Input.GetKeyDown(KeyCode.M))
			ContinueButtonPressed();
	}

	public void NewPlayerInputJoined(int playerIndex)
	{
		MenuInputManager._Instance.PlayerInputs[playerIndex].GetComponent<PlayerInput>().uiInputModule = uiInputModules[playerIndex];
		characterSelectMenus[playerIndex].GetComponent<CharacterSelectUnit>().ControllerConnected();
	}

	// Finds the UIAudioSource, and plays the button press sound
	public void ButtonPressSFX()
	{
		AudioSource buttonAudioSource = AudioManager._Instance.UIAudioSource;
		AudioManager._Instance.PlaySoundFX(AudioManager._Instance.UIAudioList[1], buttonAudioSource);
	}

	public void BackButtonPressed()
	{
		ButtonPressSFX();
		LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[0], false);
	}

	public void ContinueButtonPressed()
	{
		ButtonPressSFX();

		LevelLoadManager._Instance.StartNewGame();

		ApplyCharacterStats();

		LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[5], true);
	}

	private void ApplyCharacterStats()
	{
		for (int i = 0; i < characterSelectedByPlayers.Length; i++)
		{
			GameManager._Instance.Players[i].GetComponent<PlayerStats>().CharacterStat = characterSelectedByPlayers[i];
			if (UIColorSelectedByPlayers[i].Equals(Color.black))
			{
				GameManager._Instance.Players[i].GetComponent<PlayerStats>().PlayerColor = availableTextures[0];
				GameManager._Instance.Players[i].GetComponent<PlayerStats>().UIColor = availableColors[0];
				availableColors.RemoveAt(0);
				availableTextures.RemoveAt(0);
				characterSelectMenus[i].GetComponent<CharacterSelectUnit>().CurrentState = CharacterSelectUnit.selectState.lockedIn;
			}
			else
			{
				GameManager._Instance.Players[i].GetComponent<PlayerStats>().PlayerColor = (Texture)colorSelectedByPlayers[i];
				GameManager._Instance.Players[i].GetComponent<PlayerStats>().UIColor = uiColorSelectedByPlayers[i];
			}
		}
	}

	public void UpdateColorOptions()
	{
		availableColors = new List<Color>();
		availableTextures = new List<Texture2D>();
		bool isTaken;
		for (int i = 0; i < ListOfColors.Length; i++)
		{
			isTaken = false;
			for (int j = 0; j < UIColorSelectedByPlayers.Length; j++)
			{
				if (listOfColors[i].Equals(UIColorSelectedByPlayers[j])) isTaken = true;
			}
			if (!isTaken)
			{
				availableColors.Add(listOfColors[i]);
				availableTextures.Add(listOfTextures[i]);
			}
		}
		foreach (GameObject menu in characterSelectMenus)
		{
			menu.GetComponent<CharacterSelectUnit>().UpdateColorOptions(availableColors, availableTextures);
		}
	}
	public void CheckForLockIn()
	{
		foreach (GameObject menu in characterSelectMenus)
			if (!menu.GetComponent<CharacterSelectUnit>().CheckIfLockedIn()) return;

		ContinueButtonPressed();
	}
}
