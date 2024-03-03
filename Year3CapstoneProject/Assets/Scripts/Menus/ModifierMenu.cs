using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModifierMenu : MonoBehaviour
{
    #region SerializeFields
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject modifierDisplayObj;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private Transform modifierHolderTransform;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private List<GameObject> modifierDisplayList;

    [SerializeField, ReadOnly]
    [Foldout("Dependencies"), Tooltip("")]
    private List<Modifier> localListOfModifiers;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private int numberOfDisplaysToSpawn = 3;
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
        if (modifierDisplayList.Count == 0)
            for (int i = 0; i < numberOfDisplaysToSpawn; i++)
                modifierDisplayList.Add(GameObject.Instantiate(modifierDisplayObj, modifierHolderTransform));
        ResetAllModifierSelection();

        EventSystem.current.SetSelectedGameObject(modifierDisplayList[0].GetComponent<ModifierDisplay>().Buttonobject);
    }

    private void ResetAllModifierSelection()
    {
        // Add all the modifiers to the list
        ResetLocalModifierList();

        for (int i = 0; i < numberOfDisplaysToSpawn; i++)
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
        for (int i = 0; i < ModifierManager._Instance.ListOfModifiers.Count; i++)
            localListOfModifiers.Add(ModifierManager._Instance.ListOfModifiers[i]);
    }
}
