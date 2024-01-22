using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI Initializations")]
    [SerializeField] private string settingsMenuSceneName;
    [SerializeField, Required] private GameObject videoControls;
    [SerializeField, Required] private Image videoButton;
    [SerializeField, Required] private GameObject audioControls;
    [SerializeField, Required] private Image audioButton;
    [SerializeField, Required] private GameObject inGameControls;
    [SerializeField, Required] private Image controlsButton;
    [Header("Color Schemes")]
    [SerializeField] private Color selectedButtonColor;
    [SerializeField] private Color deselectedButtonColor;

    // Start is called before the first frame update
    void Awake()
    {
        videoButton.color = selectedButtonColor;
        audioButton.color = deselectedButtonColor;
        controlsButton.color = deselectedButtonColor;
        videoControls.SetActive(true);
        audioControls.SetActive(false);
        inGameControls.SetActive(false);
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
