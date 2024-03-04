using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class ModifierManager : MonoBehaviour
{
    // Singleton Initialization
    public static ModifierManager _Instance;

    #region SerializeFields
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("The menu to gain Modifiers")]
    private GameObject[] modifierMenus;

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

    public void CloseAllMenus()
    {
        foreach (GameObject menu in modifierMenus)
            menu.SetActive(false);
    }

    // Open Menu actions
    public void OpenModifierMenu(int playerIndexToOpen)
    {
        Time.timeScale = 0;
        modifierMenus[playerIndexToOpen].SetActive(true);
    }

    // Close Menu actions
    public void CloseModifierMenu()
    {
        Time.timeScale = 1;
        CloseAllMenus();
        playerToModify = null;

        GameManager._Instance.StartNewGame();
    }
}
