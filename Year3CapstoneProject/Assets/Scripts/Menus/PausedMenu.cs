using UnityEngine;
using UnityEngine.EventSystems;

public class PausedMenu : MenuNavigation
{
    private GameObject buttonWasOn;

    private void OnEnable()
    {
        if (EventSystem.current.gameObject != null)
            buttonWasOn = EventSystem.current.gameObject;

        EventSystem.current.SetSelectedGameObject(arrayOfbuttons[0]);
        GameManager._Instance.MenuNavigation = this;

        MenuInputManager._Instance.Reset();
        MenuInputManager._Instance.TotalNumberOfButtons = 3;

        UpdateUI(arrayOfbuttons[0]);
    }

    private void OnDisable()
    {
        EventSystem.current.SetSelectedGameObject(buttonWasOn);
    }

    // Finds the UIAudioSource, and plays the button press sound
    private void ButtonPressSFX()
    {
        AudioSource buttonAudioSource = AudioManager._Instance.UIAudioSource;
        AudioManager._Instance.PlaySoundFX(AudioManager._Instance.UIAudioList[1], buttonAudioSource);
    }
    
    public void ResumeButtonPressed()
    {
        GameManager._Instance.PauseGame(true);
    }

    public void SettingsMenuPressed()
    {
        LevelLoadManager._Instance.LoadMenuOverlay(LevelLoadManager._Instance.LevelNamesList[1]);
    }

    public void QuitButtonPressed()
    {
        GameManager._Instance.EndGame();
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[0], true);
    }

    public override void UpPressed()
    {
        ButtonPressSFX();
        MenuInputManager._Instance.moveSelection(1);
    }

    public override void DownPressed()
    {
        ButtonPressSFX();
        MenuInputManager._Instance.moveSelection(-1);
    }

    public override void LeftPressed()
    {
        ButtonPressSFX();
        MenuInputManager._Instance.moveSelection(-1);
    }

    public override void RightPressed()
    {
        ButtonPressSFX();
        MenuInputManager._Instance.moveSelection(1);
    }

    public override void SelectPressed(int buttonSelected)
    {
        ButtonPressSFX();
        switch (buttonSelected)
        {
            case 0:
                ResumeButtonPressed();
                return;
            case 1:
                QuitButtonPressed();
                //SettingsMenuPressed(); NATHANF DEBUG
                return;
            case 2:
                QuitButtonPressed();
                break;
        }
    }

    public override void CancelPressed() { ResumeButtonPressed(); }

    public override void UpdateUI(GameObject buttonSelection)
    {
        return;
    }
}
