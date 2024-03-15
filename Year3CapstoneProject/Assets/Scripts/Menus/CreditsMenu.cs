using UnityEngine;
using UnityEngine.EventSystems;
using NaughtyAttributes;

public class CreditsMenu : MonoBehaviour
{
    [Foldout("Dependencies"), Tooltip("")]
    [SerializeField, Required]
    private GameObject firstSelectedButton;

    private GameObject savedSelectedButton;

    private void Awake()
    {
        savedSelectedButton = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }

    private void OnDisable()
    {
        EventSystem.current.SetSelectedGameObject(savedSelectedButton);
    }

    public void BackButtonPressed()
    {
        LevelLoadManager._Instance.UnloadMenuOverlay(LevelLoadManager._Instance.LevelNamesList[2]);
    }
}
