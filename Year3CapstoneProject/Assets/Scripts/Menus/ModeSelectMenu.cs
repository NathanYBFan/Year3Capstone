using NaughtyAttributes;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ModeSelectMenu : MenuNavigation
{
    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private string[] modesToSelectFrom;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private TextMeshProUGUI modeTextDisplay;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private int currentSelectedMode = 0;

    private enum buttons { Mode, Continue, Back }
    private buttons selectedButton;

    private void Start()
    {
        // Setup button hookups
        EventSystem.current.SetSelectedGameObject(arrayOfbuttons[0]);
        GameManager._Instance.MenuNavigation = this;
        selectedButton = buttons.Mode;

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
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[4], true);
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
        selectedButton--;
    }

    public override void DownPressed()
    {
        selectedButton++;
    }

    public override void LeftPressed()
    {
        if (selectedButton == buttons.Mode)
        {
            LeftArrowPressed();
            return;
        }
        selectedButton--;
    }

    public override void RightPressed()
    {
        if (selectedButton == buttons.Mode)
        {
            RightArrowPressed();
            return;
        }
        selectedButton++;
    }

    public override void CancelPressed()
    {
        BackButtonPressed();
    }

    public override void UpdateUI(GameObject buttonSelected)
    {
        EventSystem.current.SetSelectedGameObject(arrayOfbuttons[(int)selectedButton]);
    }

    public override void SelectPressed(int buttonSelected)
    {
        ButtonPressSFX();
        switch (selectedButton)
        {
            case buttons.Mode:
                return; // Do nothing
            case buttons.Back:
                BackButtonPressed();
                return;
            case buttons.Continue:
                ContinueButtonPressed();
                break;
        }
    }
}
