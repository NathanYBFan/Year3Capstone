using UnityEngine;
using UnityEngine.EventSystems;

public class WinMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject firstButton;
    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
    }
    private void ButtonPressSFX()
    {
        AudioSource buttonAudioSource = AudioManager._Instance.UIAudioSource;
        AudioManager._Instance.PlaySoundFX(AudioManager._Instance.UIAudioList[1], buttonAudioSource);
    }

    public void ContinuePressed()
    {
        ButtonPressSFX();
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[0], true);
    }
}
