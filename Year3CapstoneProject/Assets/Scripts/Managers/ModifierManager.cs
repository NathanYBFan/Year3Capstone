using NaughtyAttributes;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Text holding the round counter")]
    private TextMeshProUGUI leaderboardRoundText;
    #endregion

    #region Getters&Setters
    public List<Modifier> ListOfModifiers { get { return listOfModifiers; } }
    public GameObject PlayerToModify { get {  return playerToModify; } set { playerToModify = value; } }
    #endregion

    private void Awake() // TODO NATHANF: ADD FUNCTIONALITY TO THIS SCRIPT
    {
        if (_Instance != null && _Instance != this)
        {
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
        ShowLeaderBoardMenu();
        if (MenuInputManager._Instance.MainUIEventSystem.GetComponent<EventSystem>() != null)
            MenuInputManager._Instance.MainUIEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(leaderboardButton);
    }
    public void ShowLeaderBoardMenu()
    {
		leaderboardRootObject.SetActive(true);
        leaderboardRoundText.text = "Round: " + GameManager._Instance.CurrentRound + " / " + GameManager._Instance.MaxRounds;
	}
    public void CloseLeaderBoardMenu()
    {
        GameManager._Instance.InGame = true;
        CloseAllMenus();
        GameManager._Instance.StartNewGame();
        Time.timeScale = 1;
    }
}
