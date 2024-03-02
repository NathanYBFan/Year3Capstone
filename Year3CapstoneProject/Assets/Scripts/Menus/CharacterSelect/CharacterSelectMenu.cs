 using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class CharacterSelectMenu : MonoBehaviour
{
    #region SerializeFields
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject[] characterSelectMenus = new GameObject[4];

    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("Character to instantiate")]
    private CharacterStatsSO[] characterSelectedByPlayers = new CharacterStatsSO[4];

    [SerializeField]
    [Foldout("Stats"), Tooltip("Color to assign to players")]
    private Color[] colorSelectedByPlayers = new Color[4];
    #endregion

    private void Start()
    {
        MenuInputManager._Instance.CharacterSelectMenu = this;
        for (int i = 0; i < MenuInputManager._Instance.PlayerInputs.Count; i++)
        {
            MenuInputManager._Instance.PlayerInputs[i].GetComponent<MultiplayerEventSystem>().playerRoot = characterSelectMenus[i];
            characterSelectMenus[i].GetComponent<CharacterSelectUnit>().ControllerConnected();
        }
    }

    public void NewPlayerInputJoined()
    {
        for (int i = 0; i < MenuInputManager._Instance.PlayerInputs.Count; i++)
        {
            MenuInputManager._Instance.PlayerInputs[i].GetComponent<MultiplayerEventSystem>().playerRoot = characterSelectMenus[i];
            characterSelectMenus[i].GetComponent<CharacterSelectUnit>().ControllerConnected();
        }
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
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[3], false);
    }
    
    public void ContinueButtonPressed()
    {
        ButtonPressSFX();
        
        GameManager._Instance.StartNewGame(); // Reset player stats
        LevelLoadManager._Instance.StartNewGame();

        ApplyCharacterStats();

        // Load correct scene
        if (GameManager._Instance.SelectedGameMode.CompareTo("FFA") == 0)
            LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[5], true);
        else if (GameManager._Instance.SelectedGameMode.CompareTo("TDM") == 0)
            LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[6], true);
        else if (GameManager._Instance.SelectedGameMode.CompareTo("FlatGround") == 0)
            LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[7], true);
    }

    private void ApplyCharacterStats()
    {
        for (int i = 0; i < characterSelectedByPlayers.Length; i++)
            GameManager._Instance.Players[i].GetComponent<PlayerStats>().CharacterStat = characterSelectedByPlayers[i];
    }
}
