using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class ModifierMenu : MonoBehaviour
{
	#region SerializeFields
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private GameObject firstButton;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private List<GameObject> modifierDisplayList;

	[SerializeField, ReadOnly]
	[Foldout("Dependencies"), Tooltip("")]
	private List<Modifier> localListOfModifiers;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private int playerIndex;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private InputSystemUIInputModule uiInputModule;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private InputActionAsset inputAsset;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private Image playerGlow;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private Image playerIcon;

    [SerializeField]
	[Foldout("Stats"), Tooltip("")]
	private int numberOfDisplays = 3;
	#endregion

	#region Getters&Setters
	public List<GameObject> ModifierDisplayList { get { return modifierDisplayList; } }
	#endregion

	GeneratesRumble rumble;

	private void Start()
	{
		localListOfModifiers = new List<Modifier>();
		ResetLocalModifierList();
	}

	private void OnEnable()
	{
		rumble = GetComponent<GeneratesRumble>();
		uiInputModule = transform.parent.GetChild(1).GetComponent<InputSystemUIInputModule>();

		playerIcon.sprite = GameManager._Instance.Players[playerIndex].GetComponent<PlayerStats>().CharacterStat.characterSprite;
		playerGlow.sprite = GameManager._Instance.Players[playerIndex].GetComponent<PlayerStats>().CharacterStat.characterBGSprite;
		playerIcon.color = GameManager._Instance.Players[playerIndex].GetComponent<PlayerStats>().UIColor;

		/*if (playerIndex > MenuInputManager._Instance.PlayerInputs.Count - 1)
		{
			//If we don't have 4 players and the losing player was one without input, give the control to player 1.
			uiInputModule.actionsAsset = inputAsset;
			MenuInputManager._Instance.PlayerInputs[0].GetComponent<PlayerInput>().uiInputModule = uiInputModule;
		}
		else
		{*/
		if (playerIndex < MenuInputManager._Instance.PlayerInputs.Count)
		{
			uiInputModule.actionsAsset = MenuInputManager._Instance.PlayerInputs[playerIndex].GetComponent<PlayerInput>().actions;
			MenuInputManager._Instance.PlayerInputs[playerIndex].GetComponent<PlayerInput>().uiInputModule = uiInputModule;
		}
		//}
		
		//ResetLocalModifierList();
		StartCoroutine(ResetAllModifierSelection());
		MenuInputManager._Instance.MainUIEventSystem.SetActive(false);

		StartCoroutine(GameManager._Instance.CreateRumble(rumble.RumbleDuration, rumble.LeftIntensity, rumble.RightIntensity, playerIndex, true));
		uiInputModule.GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(firstButton);
	}
	private void Update()
	{
		if (isActiveAndEnabled && Input.GetKeyDown(KeyCode.Q))
			uiInputModule.GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(firstButton);
	}

	private void OnDisable()
	{
		EventSystem.current.SetSelectedGameObject(null);
		MenuInputManager._Instance.MainUIEventSystem.SetActive(true);
	}

	private IEnumerator ResetAllModifierSelection()
	{
		// Add all the modifiers to the list
		localListOfModifiers = new List<Modifier>();
		while (!ResetLocalModifierList()) yield return null;

		for (int i = 0; i < numberOfDisplays; i++)
		{
			// Get Modifier number to choose
			int modifierSelected = Random.Range(0, localListOfModifiers.Count);
			// Get associated modifier from number
			Modifier selectedModifier = localListOfModifiers[modifierSelected];

			localListOfModifiers.RemoveAt(modifierSelected);

			modifierDisplayList[i].GetComponent<ModifierDisplay>().ResetModifier(selectedModifier, ModifierManager._Instance.PlayerToModify);
			modifierDisplayList[i].GetComponent<ModifierDisplay>().UpdateColour();

		}
	}
	public void SkipButtonPressed()
	{
		ModifierManager._Instance.CloseModifierMenu();
	}

	private bool ResetLocalModifierList()
	{
		for (int i = 0; i < ModifierManager._Instance.ListOfModifiers.Count; i++)
		{
			if (!GameManager._Instance.Players[playerIndex].GetComponent<PlayerStats>().ModifiersOnPlayer.Contains(ModifierManager._Instance.ListOfModifiers[i]))
				localListOfModifiers.Add(ModifierManager._Instance.ListOfModifiers[i]);
		}
		return true;
	}
}
