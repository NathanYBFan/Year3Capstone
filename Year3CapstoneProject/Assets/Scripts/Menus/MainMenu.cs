using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Controls;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject firstButton;

    [SerializeField]
    private GameObject trailerObject;

    [SerializeField]
    private float timeToWaitBeforeTrailer = 5; // 30 seconds

    private float counter = 0;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
        counter = 0;
    }

    private void Update()
    {
        counter += (1 * Time.deltaTime);
        if (Input.anyKey)
        {
            counter = 0;
            trailerObject.SetActive(false);
        }
        if (counter >= timeToWaitBeforeTrailer)
        {
            counter = 0;
            if (!trailerObject.activeInHierarchy)
                trailerObject.SetActive(true);
        }
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
