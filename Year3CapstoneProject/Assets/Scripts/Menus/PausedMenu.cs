using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

public class PausedMenu : MonoBehaviour
{
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Button to start on (controller support)")]
    private GameObject buttonToStartOn;

    private GameObject buttonWasOn;

    private void OnEnable()
    {
        Debug.Log("Set");
        if (EventSystem.current.gameObject != null)
            buttonWasOn = EventSystem.current.gameObject;

        EventSystem.current.SetSelectedGameObject(buttonToStartOn);
    }

    private void OnDisable()
    {
        EventSystem.current.SetSelectedGameObject(buttonWasOn);
    }

    public void ResumeButtonPressed()
    {
        GameManager._Instance.PauseGame();
    }

    public void SettingsMenuPressed()
    {
        LevelLoadManager._Instance.LoadMenuOverlay(LevelLoadManager._Instance.LevelNamesList[1]);
    }

    public void QuitButtonPressed()
    {
        GameManager._Instance.PauseGame();
        GameManager._Instance.inGame = false;
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[0], true);
    }
}
