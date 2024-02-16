using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MenuNavigation
{
    private enum buttons { PlayButton, SettingsButton, CreditsButton, QuitButton }
    private buttons selectedButton = buttons.PlayButton;

    private void Start()
    {
        selectedButton = buttons.PlayButton;
        EventSystem.current.SetSelectedGameObject(arrayOfbuttons[0]);
        GameManager._Instance.MenuNavigation = this;
        UpdateUI();
    }

    // Finds the UIAudioSource, and plays the button press sound
    private void ButtonPressSFX()
    {
        AudioSource buttonAudioSource = AudioManager._Instance.UIAudioSource;
        AudioManager._Instance.PlaySoundFX(AudioManager._Instance.UIAudioList[1], buttonAudioSource);
    }

    public override void UpdateUI()
    {
        EventSystem.current.SetSelectedGameObject(arrayOfbuttons[(int) selectedButton]);
        
        //switch (selectedButton)
        //{
        //    case buttons.PlayButton:
        //        break;
        //    case buttons.SettingsButton:
        //        EventSystem.current.SetSelectedGameObject(arrayOfbuttons[1]);
        //        break;
        //    case buttons.CreditsButton:
        //        EventSystem.current.SetSelectedGameObject(arrayOfbuttons[2]);
        //        break;
        //    case buttons.QuitButton: 
        //        EventSystem.current.SetSelectedGameObject(arrayOfbuttons[3]);
        //        break;
        //}
    }

    public override void UpPressed()
    {
        selectedButton--;
        UpdateUI();
    }

    public override void DownPressed()
    {
        selectedButton++;
        UpdateUI();
    }

    public override void LeftPressed()
    {
        selectedButton--;
        UpdateUI();
    }

    public override void RightPressed()
    {
        selectedButton++;
        UpdateUI();
    }

    public override void SelectPressed()
    {
        ButtonPressSFX();
        UpdateUI();
        switch (selectedButton)
        {
            case buttons.PlayButton:
                LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[3], true);
                return;
            case buttons.SettingsButton:
                LevelLoadManager._Instance.LoadMenuOverlay(LevelLoadManager._Instance.LevelNamesList[1]);
                return;
            case buttons.CreditsButton:
                LevelLoadManager._Instance.LoadMenuOverlay(LevelLoadManager._Instance.LevelNamesList[2]);
                break;
            case buttons.QuitButton:
                #if UNITY_STANDALONE
                Application.Quit();
                #endif

                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
                return;
        }
    }

    // No use in this class
    public override void CancelPressed() { return; }
}
