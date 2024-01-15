using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectMenu : MonoBehaviour
{
    public void BackButtonPressed()
    {
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[3]);
    }
    
    public void ContinueButtonPressed()
    {
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[5]);
    }
}
