using NaughtyAttributes;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class CharacterSelectUnit : MonoBehaviour
{
    #region SerializeFields
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private CharacterSelectMenu characterSelectMenu;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private CharacterStatsSO[] listOfStats = new CharacterStatsSO[4];

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private Color[] listOfAvailableColors;

    // Data assignment
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private TextMeshProUGUI displayCharacterName;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private Slider fireRateSlider;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private Slider moveSpeedSlider;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private Slider healthSlider;

    // Display Tab Game Objects
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject connectGameControllerObject;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject statGameObject;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject colorGameObject;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject lockInGameObject;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private int playerIndex;

    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("Character to instantiate")]
    private CharacterStatsSO characterSelectedByPlayer;

    [SerializeField]
    [Foldout("Stats"), Tooltip("Color to assign to players")]
    private Color colorSelectedByPlayer;
    #endregion

    #region PrivateVariables
    private enum selectState { connectController, characterSelect, colorSelect, lockedIn }
    private selectState currentState = selectState.characterSelect;
    private int selectedCharacter = 0;
    private int selectedColor = 0;
    #endregion

    private void Start()
    {
        selectedCharacter = 0;
        SetCharacterStatAssignment(listOfStats[playerIndex]);

        currentState = selectState.connectController;

        ResetDisplays();
        connectGameControllerObject.SetActive(true);
    }

    private void ResetDisplays()
    {
        connectGameControllerObject.SetActive(false);
        statGameObject.SetActive(false);
        colorGameObject.SetActive(false);
        lockInGameObject.SetActive(false);
    }

    private void SetCharacterStatAssignment(CharacterStatsSO characterStatToAssign)
    {
        characterSelectedByPlayer = characterStatToAssign;
        displayCharacterName.text = "--" + characterStatToAssign.CharacterName + "--";
        fireRateSlider.value = characterStatToAssign.DefaultFireRate / 10;
        moveSpeedSlider.value = characterStatToAssign.DefaultMoveSpeed / 20;
        healthSlider.value = (float)characterStatToAssign.MaxHealth / 100;
    }

    public void ControllerConnected()
    {
        currentState = selectState.characterSelect;

        ResetDisplays();
        statGameObject.SetActive(true);
    }

    private void ConfirmSelections()
    {
        
    }

    public void RightPressed()
    {
        switch (currentState)
        {
            case selectState.connectController:
                return;
            case selectState.characterSelect:
                IncrementChracterSelect(1);
                SetCharacterStatAssignment(listOfStats[selectedCharacter]);
                return;
            case selectState.colorSelect:
                IncrementColorSelect(1);
                return;
            case selectState.lockedIn:
                return;
        }
    }

    public void LeftPressed()
    {
        switch (currentState)
        {
            case selectState.connectController:
                return;
            case selectState.characterSelect:
                IncrementChracterSelect(-1);
                SetCharacterStatAssignment(listOfStats[selectedCharacter]);
                return;
            case selectState.colorSelect:
                IncrementColorSelect(-1);
                return;
            case selectState.lockedIn:
                return;
        }
    }

    private void IncrementChracterSelect(int number)
    {
        selectedCharacter += number;
        if (selectedCharacter < 0) selectedCharacter = 3;
        if (selectedCharacter > 3) selectedCharacter = 0;
    }
    private void IncrementColorSelect(int number)
    {
        selectedColor += number;
        if (selectedCharacter < 0) selectedColor = 3;
        if (selectedCharacter > 3) selectedColor = 0;
    }
}
