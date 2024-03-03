using NaughtyAttributes;
using TMPro;
using UnityEngine;
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
    private Color[] listOfColors;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private Texture2D[] listOfAvailableTextures;

    // Stats dependencies
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

    // Color dependencies
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private Image colorDisplay;

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
        SetCharacterColorAssignment(listOfAvailableTextures[0]);
        currentState = selectState.connectController;

        ResetDisplays();
        connectGameControllerObject.SetActive(true);
    }

    public void ControllerConnected()
    {
        Debug.Log("Controller connected");
        currentState = selectState.characterSelect;

        ResetDisplays();
        Debug.Log(statGameObject.name);
        statGameObject.SetActive(true);
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
        displayCharacterName.text = "--" + characterStatToAssign.CharacterName + "--";
        fireRateSlider.value = characterStatToAssign.DefaultFireRate / 10;
        moveSpeedSlider.value = characterStatToAssign.DefaultMoveSpeed / 20;
        healthSlider.value = (float)characterStatToAssign.MaxHealth / 6;
    }

    private void SetCharacterColorAssignment(Texture2D colorToSet)
    {
        colorDisplay.sprite = Sprite.Create(colorToSet, new Rect(0, 0, 50, 50), Vector2.zero);
    }

    public void ConfirmSelections()
    {
        switch (currentState)
        {
            case selectState.connectController:
                currentState = selectState.characterSelect;
                ResetDisplays();
                statGameObject.SetActive(true);
                return;
            case selectState.characterSelect:
                currentState = selectState.colorSelect;
                characterSelectMenu.CharacterSelectedByPlayers[playerIndex] = listOfStats[selectedCharacter];
                ResetDisplays();
                colorGameObject.SetActive(true);
                return;
            case selectState.colorSelect:
                currentState = selectState.lockedIn;
                characterSelectMenu.ColorSelectedByPlayers[playerIndex] = listOfAvailableTextures[selectedColor];
                characterSelectMenu.UIColorSelectedByPlayers[playerIndex] = listOfColors[selectedColor];
                ResetDisplays();
                lockInGameObject.SetActive(true);
                characterSelectMenu.CheckForLockIn();
                return;
            case selectState.lockedIn:
                return;
        }
    }

    public void CancelSelection()
    {
        switch (currentState)
        {
            case selectState.connectController:
                return;
            case selectState.characterSelect:
                characterSelectMenu.BackButtonPressed();
                return;
            case selectState.colorSelect:
                currentState = selectState.characterSelect;
                ResetDisplays();
                statGameObject.SetActive(true);
                return;
            case selectState.lockedIn:
                currentState = selectState.colorSelect;
                ResetDisplays();
                colorGameObject.SetActive(true);
                return;
        }
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
                SetCharacterColorAssignment(listOfAvailableTextures[selectedColor]);
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
                SetCharacterColorAssignment(listOfAvailableTextures[selectedColor]);
                return;
            case selectState.lockedIn:
                return;
        }
    }

    private void IncrementChracterSelect(int number)
    {
        selectedCharacter += number;
        if (selectedCharacter < 0) selectedCharacter = listOfStats.Length - 1;
        if (selectedCharacter > listOfStats.Length - 1) selectedCharacter = 0;
    }

    private void IncrementColorSelect(int number)
    {
        selectedColor += number;
        if (selectedColor < 0) selectedColor = listOfAvailableTextures.Length - 1;
        if (selectedColor > listOfAvailableTextures.Length - 1) selectedColor = 0;
    }

    public bool CheckIfLockedIn()
    {
        return currentState == selectState.lockedIn;
    }
}
