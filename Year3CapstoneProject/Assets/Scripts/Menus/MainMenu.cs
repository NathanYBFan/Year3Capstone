using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("First button to be selected - for controllers")]
    private GameObject firstButton;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Sound to play when button is pressed")]
    private AudioClip buttonSound;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void PlayGamePressed()
    {
        ButtonPressSFX();
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[3], true);
    }

    public void SettingsButtonPressed()
    {
        ButtonPressSFX();
        LevelLoadManager._Instance.LoadMenuOverlay(LevelLoadManager._Instance.LevelNamesList[1]);
    }

    public void CreditsButtonPressed()
    {
        ButtonPressSFX();
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

    //finds the UIAudioSource, and plays the button press sound
    public void ButtonPressSFX()
    {
        AudioSource buttonAudioSource = AudioManager._Instance.UIAudioSource;
        AudioManager._Instance.PlaySoundFX(AudioManager._Instance.UIAudioList[1], buttonAudioSource);
    }

}
