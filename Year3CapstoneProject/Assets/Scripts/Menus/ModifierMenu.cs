using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

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
    [Foldout("Stats"), Tooltip("")]
    private int numberOfDisplays = 3;
    #endregion

    #region Getters&Setters
    public List<GameObject> ModifierDisplayList { get { return modifierDisplayList; } }
    #endregion

    private void Start()
    {
        localListOfModifiers = new List<Modifier>();
        ResetLocalModifierList();
    }

    private void OnEnable()
    {
        ResetLocalModifierList();
        ResetAllModifierSelection();
        MenuInputManager._Instance.MainUIEventSystem.SetActive(false);
        MenuInputManager._Instance.PlayerInputs[playerIndex].GetComponent<PlayerInput>().uiInputModule = uiInputModule;

        StartCoroutine(Rumble());
        uiInputModule.GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(firstButton);
    }
    private IEnumerator Rumble()
    {
        var gamepad = MenuInputManager._Instance.PlayerInputs[playerIndex].GetComponent<PlayerInput>().GetDevice<Gamepad>();
		gamepad.ResetHaptics();
		gamepad.SetMotorSpeeds(0.25f, 0.25f);
        yield return new WaitForSecondsRealtime(0.5f);
		gamepad.ResetHaptics();
		gamepad.SetMotorSpeeds(0, 0); 
        yield break;

	}
    private void Update()
    {
        if (isActiveAndEnabled && Input.GetKeyDown(KeyCode.Q))
            uiInputModule.GetComponent<MultiplayerEventSystem>().SetSelectedGameObject(firstButton);
    }

    private void OnDisable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        // MenuInputManager._Instance.PlayerInputs[playerIndex].GetComponent<PlayerInput>().uiInputModule = null;
        MenuInputManager._Instance.MainUIEventSystem.SetActive(true);
    }

    private void ResetAllModifierSelection()
    {
        // Add all the modifiers to the list
        ResetLocalModifierList();

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

    private void ResetLocalModifierList()
    {
        localListOfModifiers = new List<Modifier>();
        for (int i = 0; i < ModifierManager._Instance.ListOfModifiers.Count; i++)
            localListOfModifiers.Add(ModifierManager._Instance.ListOfModifiers[i]);
    }
}
