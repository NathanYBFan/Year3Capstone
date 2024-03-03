using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsMenu : MonoBehaviour
{
    [Foldout("Dependencies"), Tooltip("")]
    [SerializeField]
    private string settingsMenuSceneName;
    
    [Foldout("Dependencies"), Tooltip("")]
    [SerializeField, Required]
    private GameObject videoControls;

    [Foldout("Dependencies"), Tooltip("")]
    [SerializeField, Required]
    private Image videoButton;

    [Foldout("Dependencies"), Tooltip("")]
    [SerializeField, Required]
    private GameObject audioControls;

    [Foldout("Dependencies"), Tooltip("")]
    [SerializeField, Required]
    private Image audioButton;

    [Foldout("Dependencies"), Tooltip("")]
    [SerializeField, Required]
    private GameObject firstSelectedButton;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private Color selectedButtonColor;
    
    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private Color deselectedButtonColor;

    private GameObject savedSelectedButton;
    private MenuNavigation savedMenuNavigation;

    // Start is called before the first frame update
    void Awake()
    {
        savedSelectedButton = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);

        videoControls.SetActive(true);
        audioControls.SetActive(false);
    }

    private void OnDisable()
    {
        EventSystem.current.SetSelectedGameObject(savedSelectedButton);
    }

    public void UnloadScene()
    {
        LevelLoadManager._Instance.UnloadMenuOverlay("SettingsMenu");
    }

    public void SettingsButtonPressed(int buttonNumber)
    {
        switch (buttonNumber)
        {
            case 1: // Video settings
                videoButton.color = selectedButtonColor;
                audioButton.color = deselectedButtonColor;
                videoControls.SetActive(true);
                audioControls.SetActive(false);
                break;
            case 2: // Audio settings
                videoButton.color = deselectedButtonColor;
                audioButton.color = selectedButtonColor;
                videoControls.SetActive(false);
                audioControls.SetActive(true);
                break;
            default: // Error
                Debug.Log("Invalid button selected");
                break;
        }
    }

    // Finds the UIAudioSource, and plays the button press sound
    private void ButtonPressSFX()
    {
        AudioSource buttonAudioSource = AudioManager._Instance.UIAudioSource;
        AudioManager._Instance.PlaySoundFX(AudioManager._Instance.UIAudioList[1], buttonAudioSource);
    }
}
