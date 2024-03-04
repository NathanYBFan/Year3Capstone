using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ModifierMenu : MonoBehaviour
{
    #region SerializeFields
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private List<GameObject> modifierDisplayList;

    [SerializeField, ReadOnly]
    [Foldout("Dependencies"), Tooltip("")]
    private List<Modifier> localListOfModifiers;

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
        
        // If invalid player chosen, close menu
        if (ModifierManager._Instance.PlayerToModify.GetComponent<PlayerBody>().PlayerIndex > MenuInputManager._Instance.PlayerInputs.Count)
        {
            ModifierManager._Instance.CloseModifierMenu();
            return;
        }

        // Disable every input
        foreach (GameObject input in MenuInputManager._Instance.PlayerInputs)
            input.GetComponent<PlayerInput>().DeactivateInput();

        // Enable single valid input
        MenuInputManager._Instance.PlayerInputs[ModifierManager._Instance.PlayerToModify.GetComponent<PlayerBody>().PlayerIndex - 1].GetComponent<PlayerInput>().ActivateInput();

        EventSystem.current.SetSelectedGameObject(modifierDisplayList[0].GetComponent<ModifierDisplay>().Buttonobject);
    }

    private void OnDisable()
    {
        // Reactivate all inputs
        foreach (GameObject input in MenuInputManager._Instance.PlayerInputs)
            input.GetComponent<PlayerInput>().ActivateInput();
    }

    private void ResetAllModifierSelection()
    {
        // Add all the modifiers to the list
        ResetLocalModifierList();

        for (int i = 0; i < numberOfDisplays; i++)
        {
            // Get Modifier number to choose
            int modifierSelected = Random.Range(0, localListOfModifiers.Count - 1);
            // Get associated modifier from number
            Modifier selectedModifier = localListOfModifiers[modifierSelected];
            
            localListOfModifiers.RemoveAt(modifierSelected);

            modifierDisplayList[i].GetComponent<ModifierDisplay>().ResetModifier(selectedModifier, ModifierManager._Instance.PlayerToModify);
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
