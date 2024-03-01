using NaughtyAttributes;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ModeSelectMenu : MenuNavigation
{
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("DEBUG TO BE DELETED")] // TODO NATHAN F: REMOVE THIS
    private CharacterStatsSO[] listOfStats = new CharacterStatsSO[4];

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private string[] modesToSelectFrom;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private TextMeshProUGUI modeTextDisplay;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private int currentSelectedMode = 0;

    private void Start()
    {
        // Setup button hookups
        EventSystem.current.SetSelectedGameObject(arrayOfbuttons[0]);
        GameManager._Instance.MenuNavigation = this;

        MenuInputManager._Instance.Reset();
        MenuInputManager._Instance.TotalNumberOfButtons = 2;

        UpdateUI(arrayOfbuttons[0]);

        // Check if a mode is already selected
        for (int i = 0; i < modesToSelectFrom.Length; i++)
        {
            if (GameManager._Instance.SelectedGameMode.CompareTo(modesToSelectFrom[i]) == 0)
            {
                currentSelectedMode = i;
                modeTextDisplay.text = modesToSelectFrom[currentSelectedMode];
                return;
            }
        }
        modeTextDisplay.text = modesToSelectFrom[currentSelectedMode];
    }

    public void LeftArrowPressed()
    {
        currentSelectedMode -= 1;
        if (currentSelectedMode < 0)
            currentSelectedMode = modesToSelectFrom.Length - 1;
        modeTextDisplay.text = modesToSelectFrom[currentSelectedMode];
    }

    public void RightArrowPressed()
    {
        currentSelectedMode += 1;
        if (currentSelectedMode > modesToSelectFrom.Length - 1)
            currentSelectedMode = 0;
        modeTextDisplay.text = modesToSelectFrom[currentSelectedMode];
    }

    public void ContinueButtonPressed()
    {
        GameManager._Instance.SelectedGameMode = modesToSelectFrom[currentSelectedMode];
        // LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[4], true);

        // ALL BELOW IS DEBUG
        GameManager._Instance.StartNewGame(); // Reset player stats
        LevelLoadManager._Instance.StartNewGame();

        for (int i = 0; i < listOfStats.Length; i++)
            GameManager._Instance.Players[i].GetComponent<PlayerStats>().CharacterStat = listOfStats[i];

        // Load correct scene
        if (GameManager._Instance.SelectedGameMode.CompareTo("FFA") == 0)
            LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[5], true);
        else if (GameManager._Instance.SelectedGameMode.CompareTo("TDM") == 0)
            LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[6], true);
        else if (GameManager._Instance.SelectedGameMode.CompareTo("FlatGround") == 0)
            LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[7], true);
    }

    public void BackButtonPressed()
    {
        GameManager._Instance.SelectedGameMode = modesToSelectFrom[currentSelectedMode];
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[0], false);
    }


    // Finds the UIAudioSource, and plays the button press sound
    public void ButtonPressSFX()
    {
        AudioSource buttonAudioSource = AudioManager._Instance.UIAudioSource;
        AudioManager._Instance.PlaySoundFX(AudioManager._Instance.UIAudioList[1], buttonAudioSource);
    }

    public override void UpPressed()
    {
        ButtonPressSFX();
        MenuInputManager._Instance.moveSelection(-1);
    }

    public override void DownPressed()
    {
        ButtonPressSFX(); 
        MenuInputManager._Instance.moveSelection(1);
    }

    public override void LeftPressed()
    {
        ButtonPressSFX();
        LeftArrowPressed();
    }

    public override void RightPressed()
    {
        ButtonPressSFX();
        RightArrowPressed();
    }

    public override void CancelPressed()
    {
        BackButtonPressed();
    }

    public override void UpdateUI(GameObject buttonSelected)
    {
        // EventSystem.current.SetSelectedGameObject(arrayOfbuttons[(int)selectedButton]);
    }

    public override void SelectPressed(int buttonSelected)
    {
        ButtonPressSFX();
        switch (buttonSelected)
        {
            case 0:
                ContinueButtonPressed();
                return;
            case 1:
                BackButtonPressed();
                return;
        }
    }
}
