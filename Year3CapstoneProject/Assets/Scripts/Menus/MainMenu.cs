using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject firstButton;

    [SerializeField]
    private GameObject trailerObject;

    [SerializeField]
    private GameObject quitTrailerButton;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void TrailerButtonPressed()
    {
        ButtonPressSFX();
        trailerObject.SetActive(true);
        AudioManager._Instance.MuteMusic(true);
        EventSystem.current.SetSelectedGameObject(quitTrailerButton);
    }

    public void CloseTrailerPressed()
    {
        ButtonPressSFX();
        trailerObject.SetActive(false);
        AudioManager._Instance.MuteMusic(false);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    // Finds the UIAudioSource, and plays the button press sound
    private void ButtonPressSFX()
    {
        AudioSource buttonAudioSource = AudioManager._Instance.UIAudioSource;
        AudioManager._Instance.PlaySoundFX(AudioManager._Instance.UIAudioList[1], buttonAudioSource);
    }

    public void PlayGamePressed()
    {
        ButtonPressSFX();
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[4], true);
    }

    public void SettingsButtonPressed()
    {
        ButtonPressSFX();
        LevelLoadManager._Instance.LoadMenuOverlay(LevelLoadManager._Instance.LevelNamesList[1]);
    }

    public void ControlsButtonPressed()
    {
        ButtonPressSFX();
        LevelLoadManager._Instance.LoadMenuOverlay(LevelLoadManager._Instance.LevelNamesList[8]);
    }

    public void CreditsbuttonPressed()
    {
        ButtonPressSFX();
        LevelLoadManager._Instance.LoadMenuOverlay(LevelLoadManager._Instance.LevelNamesList[2]);
    }

    public void QuitButtonPressed()
    {
        ButtonPressSFX();
#if UNITY_STANDALONE
        Application.Quit();
#endif

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
