using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

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
    [Foldout("Stats"), Tooltip("Color to assign to players")]
    private Texture[] colorSelectedByPlayers = new Texture[4];

    [SerializeField]
    [Foldout("Stats"), Tooltip("Color to assign to players")]
    private Color[] uiColorSelectedByPlayers = new Color[4];
    #endregion

    public CharacterStatsSO[] CharacterSelectedByPlayers { get { return characterSelectedByPlayers; } set { characterSelectedByPlayers = value; } }
    public Texture[] ColorSelectedByPlayers { get { return colorSelectedByPlayers; } set { colorSelectedByPlayers = value; } }
    public Color[] UIColorSelectedByPlayers { get { return uiColorSelectedByPlayers; } set { uiColorSelectedByPlayers = value; } }
    public GameObject[] CharacterSelectMenus { get { return characterSelectMenus; } set { characterSelectMenus = value; } }

    private void Start()
    {
        MenuInputManager._Instance.InCharacterSelect = true;
        MenuInputManager._Instance.CharacterSelectMenu = this;

        for (int i = 0; i < MenuInputManager._Instance.PlayerInputs.Count; i++)
            NewPlayerInputJoined(i);
        MenuInputManager._Instance.EnterCharacterSelectScreen();
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
            GameManager._Instance.Players[i].GetComponent<PlayerStats>().PlayerColor = (Texture)colorSelectedByPlayers[i];
            GameManager._Instance.Players[i].GetComponent<PlayerStats>().UIColor = uiColorSelectedByPlayers[i];
        }
    }

    public void CheckForLockIn()
    {
        foreach(GameObject menu in characterSelectMenus)
            if (!menu.GetComponent<CharacterSelectUnit>().CheckIfLockedIn()) return;

        ContinueButtonPressed();
    }
}
