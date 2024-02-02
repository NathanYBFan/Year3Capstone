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
    private GameObject inGameControls;

    [Foldout("Dependencies"), Tooltip("")]
    [SerializeField, Required]
    private Image controlsButton;

    [Foldout("Dependencies"), Tooltip("")]
    [SerializeField, Required]
    private GameObject firstSelectedButton;

    [SerializeField, ReadOnly]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject savedSelectedButton;


    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private Color selectedButtonColor;
    
    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private Color deselectedButtonColor;

    // Start is called before the first frame update
    void Awake()
    {
        savedSelectedButton = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        videoButton.color = selectedButtonColor;
        audioButton.color = deselectedButtonColor;
        controlsButton.color = deselectedButtonColor;
        videoControls.SetActive(true);
        audioControls.SetActive(false);
        inGameControls.SetActive(false);
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
                controlsButton.color = deselectedButtonColor;
                videoControls.SetActive(true);
                audioControls.SetActive(false);
                inGameControls.SetActive(false);
                break;
            case 2: // Audio settings
                videoButton.color = deselectedButtonColor;
                audioButton.color = selectedButtonColor;
                controlsButton.color = deselectedButtonColor;
                videoControls.SetActive(false);
                audioControls.SetActive(true);
                inGameControls.SetActive(false);
                break;
            case 3: // Control settings
                videoButton.color = deselectedButtonColor;
                audioButton.color = deselectedButtonColor;
                controlsButton.color = selectedButtonColor;
                videoControls.SetActive(false);
                audioControls.SetActive(false);
                inGameControls.SetActive(true);
                break;
            default: // Error
                Debug.Log("Invalid button selected");
                break;
        }
    }
}