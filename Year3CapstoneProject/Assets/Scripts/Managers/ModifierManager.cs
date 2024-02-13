using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class ModifierManager : MonoBehaviour
{
    // Singleton Initialization
    public static ModifierManager _Instance;

    #region SerializeFields
    [SerializeField, Required]
    [Foldout("Dependencies"), Tooltip("The menu to gain Modifiers")]
    private GameObject modifierMenu;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("List of all modifiers obtainable in the game")]
    private List<Modifier> listOfModifiers;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("The player to apply the modifier to")]
    private GameObject playerToModify;
    #endregion

    #region Getters&Setters
    public List<Modifier> ListOfModifiers { get { return listOfModifiers; } }
    public GameObject PlayerToModify { get {  return playerToModify; } set { playerToModify = value; } }
    #endregion

    private void Awake() // TODO NATHANF: ADD FUNCTIONALITY TO THIS SCRIPT
    {
        if (_Instance != null && _Instance != this)
        {
            Debug.LogWarning("Destroyed a repeated ModifierManager");
            Destroy(this.gameObject);
        }
        else if (_Instance == null)
            _Instance = this;
    }

    // Open Menu actions
    public void OpenModifierMenu()
    {
        Time.timeScale = 0;
        modifierMenu.SetActive(true);
    }

    // Close Menu actions
    public void CloseModifierMenu()
    {
        Time.timeScale = 1;
        modifierMenu.SetActive(false);
        playerToModify = null;
    }
}
