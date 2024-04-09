using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("The player to apply the modifier to")]
    private GameObject leaderboardRootObject;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("The player to apply the modifier to")]
    private GameObject leaderboardButton;
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
        Time.timeScale = 0;
        foreach (GameObject menu in modifierMenus)
            menu.SetActive(false);
        leaderboardRootObject.SetActive(false);
    }

    // Open Menu actions
    public void OpenModifierMenu(int playerIndexToOpen)
    {
        GameManager._Instance.InGame = false;
        CloseAllMenus();
        modifierMenus[playerIndexToOpen].SetActive(true);
    }

    // Close Menu actions
    public void CloseModifierMenu()
    {
        CloseAllMenus();
        playerToModify = null;
        leaderboardRootObject.SetActive(true);
        MenuInputManager._Instance.MainUIEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(leaderboardButton);
    }
    public void ShowLeaderBoardMenu()
    {
		leaderboardRootObject.SetActive(true);
	}
    public void CloseLeaderBoardMenu()
    {
        GameManager._Instance.InGame = true;
        CloseAllMenus();
        GameManager._Instance.StartNewGame();
        Time.timeScale = 1;
    }
}
