using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterSelectMenu : MonoBehaviour
{
    #region SerializeFields
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private CharacterStatsSO[] listOfStats = new CharacterStatsSO[4];

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject[] displayParent = new GameObject[4];

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("First button to be selected - for controllers")]
    private GameObject firstButton;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject[] statGameObjects;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject[] ColorGameObjects;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject[] lockInGameObjects;

    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("Character to instantiate")]
    private CharacterStatsSO[] characterSelectedByPlayers = new CharacterStatsSO[4];

    [SerializeField]
    [Foldout("Stats"), Tooltip("Color to assign to players")]
    private Color[] colorSelectedByPlayers = new Color[4];
    #endregion

    #region PrivateVariables
    private enum selectState { characterSelect, colorSelect, lockedIn }
    private selectState currentState = selectState.characterSelect;
    #endregion

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
        for (int i = 0; i < characterSelectedByPlayers.Length; i++)
            SetCharacterStatAssignment(i, listOfStats[i]);
        currentState = selectState.characterSelect;
        UpdateDisplayUI();
    }

    private void SetCharacterStatAssignment(int characterIndex, CharacterStatsSO characterStatToAssign)
    {
        characterSelectedByPlayers[characterIndex] = characterStatToAssign;
        Debug.Log(displayParent[characterIndex].name);
        displayParent[characterIndex].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "--" + characterStatToAssign.CharacterName + "--";
        displayParent[characterIndex].transform.GetChild(1).GetChild(0).GetComponentInChildren<Slider>().value = characterStatToAssign.DefaultFireRate / 10;
        displayParent[characterIndex].transform.GetChild(1).GetChild(1).GetComponentInChildren<Slider>().value = characterStatToAssign.DefaultMoveSpeed / 20;
        displayParent[characterIndex].transform.GetChild(1).GetChild(2).GetComponentInChildren<Slider>().value = (float)characterStatToAssign.MaxHealth / 100;
    }

    private void SetColorAssignment(int colorIndex)
    {

    }

    private void ConfirmSelected()
    {
        switch (currentState)
        {
            case selectState.characterSelect:

                currentState = selectState.colorSelect;
                break;
            case selectState.colorSelect:

                currentState = selectState.lockedIn;
                break;
            case selectState.lockedIn: return; // Do nothing
            default:
                Debug.Log("Error case reached, current state is null");
                break;
        }
    }

    private void CancelSelected()
    {
        switch (currentState)
        {
            case selectState.characterSelect: return; // Do nothing
            case selectState.colorSelect:
                currentState = selectState.characterSelect;
                break;
            case selectState.lockedIn:
                currentState = selectState.colorSelect;
                break;
            default:
                Debug.Log("Error case reached, current state is null");
                break;
        }
        UpdateDisplayUI();
    }

    private void UpdateDisplayUI()
    {
        // Turn all UI off
        ResetAllUI();

        // Turn on depending on state
        switch (currentState)
        {
            case selectState.characterSelect:
                foreach (GameObject stat in statGameObjects)
                    stat.SetActive(true);
                return;
            case selectState.colorSelect:
                foreach (GameObject color in ColorGameObjects)
                    color.SetActive(true);
                return;
            case selectState.lockedIn:
                foreach (GameObject lockIn in lockInGameObjects)
                    lockIn.SetActive(true);
                return;
            default:
                Debug.Log("Error case reached, current state is null");
                break;
        }
    }

    private void ResetAllUI()
    {
        foreach (GameObject stat in statGameObjects)
            stat.SetActive(false);
        foreach (GameObject color in ColorGameObjects)
            color.SetActive(false);
        foreach (GameObject lockIn in lockInGameObjects)
            lockIn.SetActive(false);
    }

    public void BackButtonPressed()
    {
        AudioManager._Instance.PlaySoundFX(AudioManager._Instance.UIAudioList[1], AudioManager._Instance.UIAudioSource);
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[3], false);
    }
    
    public void ContinueButtonPressed()
    {
        AudioManager._Instance.PlaySoundFX(AudioManager._Instance.UIAudioList[1], AudioManager._Instance.UIAudioSource);
        
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
