using NaughtyAttributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUnit : MonoBehaviour
{
    #region SerializeFields
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Out of prefab controller")]
    private CharacterSelectMenu characterSelectMenu;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Out of prefab characterDisplay")]
    private PlayerDisplay characterDisplay;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("List of all possible stats to choose from")]
    private CharacterStatsSO[] listOfStats = new CharacterStatsSO[4];

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("List of all possible colors to choose from")]
	private Color[] availableColors;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("List of all possible textures to choose from (Link to colors)")]
	private Texture2D[] listOfAvailableTextures;

    // Stats dependencies
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Character name to change")]
    private TextMeshProUGUI displayCharacterName;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Stats vs Pick Color text")]
    private TextMeshProUGUI sideBarTitle;

    // Stat Bars
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Array of 3 bar gameobjects")]
    private GameObject[] fireRateBars = new GameObject[3];

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Array of 3 bar gameobjects")]
    private GameObject[] moveSpeedBars = new GameObject[3];

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Array of 3 bar gameobjects")]
    private GameObject[] healthAmountBars = new GameObject[3];


    // Color dependencies
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private Image colorDisplay;

    //Icon Dependencies
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Character icon")]
    private Image iconDisplay;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Character bg icon")]
    private Image iconBgDisplay;

    // Display Tab Game Objects
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Root game object holding the connect game controller display objects")]
    private GameObject connectGameControllerObject;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Root game object holding the stat display objects")]
    private GameObject statGameObject;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Root game object holding the color select display objects")]
    private GameObject colorGameObject;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Root game object holding the lock in display objects")]
    private GameObject lockInGameObject;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private int playerIndex;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private int selectedCharacter = 0;
    #endregion

    #region PrivateVariables
    public enum selectState { connectController, characterSelect, colorSelect, lockedIn }
    private selectState currentState = selectState.characterSelect;
    
    private int selectedColor = 0;
	#endregion


	public selectState CurrentState { get { return currentState; } set { currentState = value; } }

	private void Start()
	{
		SetCharacterStatAssignment(listOfStats[playerIndex]);
        currentState = selectState.connectController;

        ResetDisplays();
        connectGameControllerObject.SetActive(true);
    }

    public void UpdateColorOptions(List<Color> colors, List<Texture2D> textures)
    {
        if (currentState == selectState.lockedIn) return;
        availableColors = colors.ToArray();
        listOfAvailableTextures = textures.ToArray();
        SetCharacterColorAssignment(availableColors[selectedColor], listOfAvailableTextures[selectedColor]);
    }
    public void ControllerConnected()
    {
        currentState = selectState.connectController;

        ResetDisplays();
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
        iconDisplay.sprite = characterStatToAssign.characterSprite;
        iconBgDisplay.sprite = characterStatToAssign.characterBGSprite;
        displayCharacterName.text = "--" + characterStatToAssign.CharacterName + "--";

        int numBarsToActivate = 0;

        // FireRate
        for (int i = 0; i < fireRateBars.Length; i++)
            fireRateBars[i].SetActive(i < (int)characterStatToAssign.DefaultFireRate - 2);
        
        // MoveSpeed
        if (characterStatToAssign.DefaultMoveSpeed >= 17)
            numBarsToActivate = 3;
        else if (characterStatToAssign.DefaultMoveSpeed == 14)
            numBarsToActivate = 2;
        else if (characterStatToAssign.DefaultMoveSpeed <= 11)
            numBarsToActivate = 1;
        for (int i = 0; i < moveSpeedBars.Length; i++)
            moveSpeedBars[i].SetActive(i < numBarsToActivate);

        // Health
        if (characterStatToAssign.MaxHealth >= 6)
            numBarsToActivate = 3;
        else if (characterStatToAssign.MaxHealth == 4)
            numBarsToActivate = 2;
        else if (characterStatToAssign.MaxHealth <= 3)
            numBarsToActivate = 1;
        for (int i = 0; i < healthAmountBars.Length; i++)
            healthAmountBars[i].SetActive(i < numBarsToActivate);
    }

    private void SetCharacterColorAssignment(Color colorToSet, Texture colorTexture)
    {
        colorDisplay.color = colorToSet;
        iconDisplay.color = colorToSet;
        characterDisplay.ResetMaterialEmissionColor(playerIndex, colorTexture, availableColors[selectedColor]);
    }

    public void ConfirmSelections()
    {
        switch (currentState)
        {
            case selectState.connectController:
                currentState = selectState.characterSelect;
                ResetDisplays();
                sideBarTitle.text = "Stats:";
                statGameObject.SetActive(true);
                return;
            case selectState.characterSelect:
                currentState = selectState.colorSelect;
                characterSelectMenu.CharacterSelectedByPlayers[playerIndex] = listOfStats[selectedCharacter];
                ResetDisplays();
                sideBarTitle.text = "Pick Your Color:";
                colorGameObject.SetActive(true);
                return;
            case selectState.colorSelect:
                currentState = selectState.lockedIn;
                characterSelectMenu.ColorSelectedByPlayers[playerIndex] = listOfAvailableTextures[selectedColor];
                characterSelectMenu.UIColorSelectedByPlayers[playerIndex] = availableColors[selectedColor];
				characterSelectMenu.UpdateColorOptions();
                ResetDisplays();
                sideBarTitle.text = "";
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
                sideBarTitle.text = "";
                return;
            case selectState.colorSelect:
                currentState = selectState.characterSelect;
				ResetDisplays();
                sideBarTitle.text = "Stats:";
                statGameObject.SetActive(true);
                return;
            case selectState.lockedIn:
                currentState = selectState.colorSelect;
				characterSelectMenu.ColorSelectedByPlayers[playerIndex] = null;
				characterSelectMenu.UIColorSelectedByPlayers[playerIndex] = Color.black;
				characterSelectMenu.UpdateColorOptions();
				ResetDisplays();
                sideBarTitle.text = "Pick Your Color:";
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
                SetCharacterColorAssignment(availableColors[selectedColor], listOfAvailableTextures[selectedColor]);
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
                SetCharacterColorAssignment(availableColors[selectedColor], listOfAvailableTextures[selectedColor]);
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
        characterDisplay.UpdateCharacterDisplay(playerIndex, selectedCharacter);
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
