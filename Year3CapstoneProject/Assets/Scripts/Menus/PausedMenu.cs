using UnityEngine;
using UnityEngine.EventSystems;

public class PausedMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject firstButton;

    private GameObject buttonWasOn;

    private void OnEnable()
    {
        GameManager._Instance.InPauseMenu = true;
        if (EventSystem.current.gameObject != null)
            buttonWasOn = EventSystem.current.gameObject;

        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    private void OnDisable()
    {
        GameManager._Instance.InPauseMenu = false;
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
        ButtonPressSFX();
        GameManager._Instance.PauseGame(true);
    }

    public void SettingsMenuPressed()
    {
        ButtonPressSFX();
        LevelLoadManager._Instance.LoadMenuOverlay(LevelLoadManager._Instance.LevelNamesList[1]);
    }

    public void QuitButtonPressed()
    {
        ButtonPressSFX();
        GameManager._Instance.EndGame();
        GameManager._Instance.IsPaused = false;
		foreach (GameObject hud in GameManager._Instance.HudBars)
			hud.GetComponent<Bars>().EarlyOut = false;
		LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[0], true);
    }
}
