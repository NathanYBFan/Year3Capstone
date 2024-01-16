using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class ModifierMenu : MonoBehaviour
{
    // Serialize Fields
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject modifierDisplayObj;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private Transform modifierHolderTransform;

    [SerializeField, ReadOnly]
    [Foldout("Dependencies"), Tooltip("")]
    private List<GameObject> modifierDisplayList;

    // Getters
    public List<GameObject> ModifierDisplayList { get { return modifierDisplayList; } }

    private void Start()
    {
        for (int i = 0; i < 3; i++)
            modifierDisplayList.Add(GameObject.Instantiate(modifierDisplayObj, modifierHolderTransform));
        ModifierManager._Instance.CloseModifierMenu();
    }

    private void OnEnable()
    {
        ResetAllModifierSelection();
    }

    private void ResetAllModifierSelection()
    {
        List<GameObject> localListOfModifiers = ModifierManager._Instance.ListOfModifiers;

        foreach (GameObject modiferDisplay in modifierDisplayList)
        {
            int modifierSelected = Random.Range(0, localListOfModifiers.Count - 1);
            Modifier selectedModifier = localListOfModifiers[modifierSelected].GetComponent<Modifier>();
            localListOfModifiers.RemoveAt(modifierSelected);

            modiferDisplay.GetComponent<ModifierDisplay>().ResetModifier(selectedModifier);
        }
    }

    public void SkipButtonPressed()
    {
        ModifierManager._Instance.CloseModifierMenu();
    }
}
