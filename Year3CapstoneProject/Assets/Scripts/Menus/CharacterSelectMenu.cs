using NaughtyAttributes;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectMenu : MonoBehaviour
{
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private CharacterStatsSO[] listOfStats = new CharacterStatsSO[4];

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private CharacterStatsSO[] characterSelectedByPlayers = new CharacterStatsSO[4];

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject[] displayParent = new GameObject[4];

    private void Start()
    {
        for (int i = 0; i < listOfStats.Count(); i++)
            SetCharacterStatAssignment(i, listOfStats[i]);
    }

    private void SetCharacterStatAssignment(int characterIndex, CharacterStatsSO characterStatToAssign)
    {
        characterSelectedByPlayers[characterIndex] = characterStatToAssign;
        displayParent[characterIndex].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "--" + characterStatToAssign.CharacterName + "--";
        displayParent[characterIndex].transform.GetChild(0).GetChild(1).GetChild(0).GetComponentInChildren<Slider>().value = characterStatToAssign.DefaultFireRate / 10;
        displayParent[characterIndex].transform.GetChild(0).GetChild(1).GetChild(1).GetComponentInChildren<Slider>().value = characterStatToAssign.DefaultMoveSpeed / 20;
        displayParent[characterIndex].transform.GetChild(0).GetChild(1).GetChild(2).GetComponentInChildren<Slider>().value = (float) characterStatToAssign.MaxHealth / 100;
    }

    public void BackButtonPressed()
    {
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[3], false);
    }
    
    public void ContinueButtonPressed()
    {
        GameManager._Instance.StartNewGame(); // Reset player stats
        LevelLoadManager._Instance.StartNewGame();

        ApplyCharacterStats();



        // Load correct scene
        if (GameManager._Instance.SelectedGameMode.CompareTo("WHITEBOX") == 0)
            LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[5], true);
        else if(GameManager._Instance.SelectedGameMode.CompareTo("STAGE CREATION") == 0)
            LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[7], true);
        else if (GameManager._Instance.SelectedGameMode.CompareTo("MODELS") == 0)
            LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[6], true);
    }

    private void ApplyCharacterStats()
    {
        for (int i = 0; i < characterSelectedByPlayers.Length; i++)
        {
            GameManager._Instance.Players[i].GetComponent<PlayerStats>().ApplyStats(characterSelectedByPlayers[i]);
        }
    } 
}
