using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MenuNavigation
{
    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(arrayOfbuttons[0]);
        GameManager._Instance.MenuNavigation = this;
        MenuInputManager._Instance.Reset();
        MenuInputManager._Instance.TotalNumberOfButtons = 4;
        UpdateUI(arrayOfbuttons[0]);
    }

    // Finds the UIAudioSource, and plays the button press sound
    private void ButtonPressSFX()
    {
        AudioSource buttonAudioSource = AudioManager._Instance.UIAudioSource;
        AudioManager._Instance.PlaySoundFX(AudioManager._Instance.UIAudioList[1], buttonAudioSource);
    }

    public override void UpdateUI(GameObject selection)
    {
        // EventSystem.current.SetSelectedGameObject(selection);
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
                PlayGamePressed();
                return;
            case 1:
                SettingsButtonPressed();
                return;
            case 2:
                CreditsbuttonPressed();
                break;
            case 3:
                QuitButtonPressed();
                return;
        }
    }

    // No use in this class
    public override void CancelPressed() { return; }

    public void PlayGamePressed()
    {
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[3], true);
    }

    public void SettingsButtonPressed()
    {
        LevelLoadManager._Instance.LoadMenuOverlay(LevelLoadManager._Instance.LevelNamesList[1]);
    }

    public void CreditsbuttonPressed()
    {
        LevelLoadManager._Instance.LoadMenuOverlay(LevelLoadManager._Instance.LevelNamesList[2]);
    }

    public void QuitButtonPressed()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
