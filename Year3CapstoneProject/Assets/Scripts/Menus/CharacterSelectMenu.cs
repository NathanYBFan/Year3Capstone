using UnityEngine;

public class CharacterSelectMenu : MonoBehaviour
{
    public void BackButtonPressed()
    {
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[3], false);
    }
    
    public void ContinueButtonPressed()
    {
        GameManager._Instance.StartNewGame(); // Reset player stats
        LevelLoadManager._Instance.StartNewGame();

        if (GameManager._Instance.SelectedGameMode.CompareTo("WHITEBOX") == 0)
            LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[5], true);
        else if(GameManager._Instance.SelectedGameMode.CompareTo("WHITEBOX") == 0)
            LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[5], true);


    }
}
