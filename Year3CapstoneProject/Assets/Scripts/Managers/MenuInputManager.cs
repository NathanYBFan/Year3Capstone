 using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuInputManager : MonoBehaviour
{
    public static MenuInputManager _Instance;

    #region SerializeFields
    [SerializeField]
    private int currentButton;

    [SerializeField, ReadOnly]
    private List<GameObject> playerInputs;

    [SerializeField]
    private EventSystem mainUIEventSystem;
    #endregion
    
    #region PublicVariables
    public List<GameObject> PlayerInputs { get { return playerInputs; } set { playerInputs = value; } }
    public bool InCharacterSelect { get { return inCharacterSelect; } set { inCharacterSelect = value; } }
    public CharacterSelectMenu CharacterSelectMenu { get { return characterSelectMenu; } set { characterSelectMenu = value; } }
    #endregion

    private bool inCharacterSelect = false;
    private CharacterSelectMenu characterSelectMenu;

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Debug.LogWarning("Destroyed a repeated MenuInput Manager");
            Destroy(this.gameObject);
        }

        else if (_Instance == null)
            _Instance = this;
    }

    public void EnterCharacterSelectScreen()
    {
        mainUIEventSystem.gameObject.SetActive(false);
    }

    public void ExitCharacterSelectScreen()
    {
        mainUIEventSystem.gameObject.SetActive(true);
    }

    public void ControllerRejoinEvent(int playerIndex)
    {
        if (!inCharacterSelect) return;

        characterSelectMenu.NewPlayerInputJoined(playerIndex);

        mainUIEventSystem.gameObject.SetActive(true);
    }
}
