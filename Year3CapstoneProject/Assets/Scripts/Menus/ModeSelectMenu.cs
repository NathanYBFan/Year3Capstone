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

    private void Start()
    {
        // Setup button hookups
        EventSystem.current.SetSelectedGameObject(arrayOfbuttons[0]);
        GameManager._Instance.MenuNavigation = this;

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
        ButtonPressSFX();
        GameManager._Instance.SelectedGameMode = modesToSelectFrom[currentSelectedMode];
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[4], true);
    }

    public void BackButtonPressed()
    {
        ButtonPressSFX();
        GameManager._Instance.SelectedGameMode = modesToSelectFrom[currentSelectedMode];
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[0], false);
    }


    //finds the UIAudioSource, and plays the button press sound
    public void ButtonPressSFX()
    {
        AudioSource buttonAudioSource = AudioManager._Instance.UIAudioSource;
        AudioManager._Instance.PlaySoundFX(AudioManager._Instance.UIAudioList[1], buttonAudioSource);
    }

    public override void UpPressed()
    {
        throw new System.NotImplementedException();
    }

    public override void DownPressed()
    {
        throw new System.NotImplementedException();
    }

    public override void LeftPressed()
    {
        throw new System.NotImplementedException();
    }

    public override void RightPressed()
    {
        throw new System.NotImplementedException();
    }

    public override void SelectPressed()
    {
        throw new System.NotImplementedException();
    }

    public override void CancelPressed()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateUI()
    {
        throw new System.NotImplementedException();
    }
}
