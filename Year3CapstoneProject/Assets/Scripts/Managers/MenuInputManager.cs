using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class MenuInputManager : MonoBehaviour
{
    public static MenuInputManager _Instance;

    #region SerializeFields
    [SerializeField]
    private int currentButton;

    [SerializeField, ReadOnly]
    private List<GameObject> playerInputs;
    #endregion
    
    #region PublicVariables
    public int TotalNumberOfButtons { get { return totalNumberOfbuttons; } set { totalNumberOfbuttons = value; } }
    public int CurrentButton { get { return currentButton; } }
    public List<GameObject> PlayerInputs { get { return playerInputs; } set { playerInputs = value; } }
    public MenuNavigation MenuNavigation { get { return menuNavigation; } set { menuNavigation = value; } }
    public bool InCharacterSelect { get { return inCharacterSelect; } set { inCharacterSelect = value; } }
    public CharacterSelectMenu CharacterSelectMenu { get { return characterSelectMenu; } set { characterSelectMenu = value; } }
    #endregion

    private int totalNumberOfbuttons;
    private MenuNavigation menuNavigation;
    private bool inCharacterSelect = false;
    private CharacterSelectMenu characterSelectMenu;
    
    private void Awake()
    {
        inCharacterSelect = false;
        if (_Instance != null && _Instance != this)
        {
            Debug.LogWarning("Destroyed a repeated MenuInput Manager");
            Destroy(this.gameObject);
        }

        else if (_Instance == null)
            _Instance = this;
    }


    public void Reset()
    {
        currentButton = 0;
    }

    public void moveSelection(int button)
    {
        currentButton += button;

        if (currentButton < 0) currentButton = totalNumberOfbuttons - 1;
        else if (currentButton >= totalNumberOfbuttons) currentButton = 0;

        // Highlight selected button
        menuNavigation.UpdateUI(menuNavigation.arrayOfbuttons[currentButton]);
    }

    public void ConfirmSelection()
    {
        menuNavigation.SelectPressed(currentButton);
    }

    public void CancelSelection()
    {
        menuNavigation.CancelPressed();
    }

    public void ControllerRejoinEvent(int playerIndex)
    {
        if (!inCharacterSelect) return;

        //characterSelectMenu.NewPlayerInputJoined(playerIndex);
    }
}
