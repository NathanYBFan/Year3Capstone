using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGamePressed()
    {
        // TODO NATHANF: SET LEVELNAMELIST[i] TO CORRECT NUMBER
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[3]);
        // Reset player stats
        GameManager._Instance.StartNewGame();
    }

    // TODO NATHANF: ADJUST SETTINGS TO WORK WITH LEVEL LOADER
    public void SettingsButtonPressed()
    {
        LevelLoadManager._Instance.LoadMenuOverlay("SettingsMenu");
    }

    public void CreditsButtonPressed()
    {
        LevelLoadManager._Instance.LoadMenuOverlay("CreditsMenu");
    }

    public void QuitButtonPressed()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
