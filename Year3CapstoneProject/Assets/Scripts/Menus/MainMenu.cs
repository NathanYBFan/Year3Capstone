using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGamePressed()
    {
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[3], true);
    }

    // TODO NATHANF: ADJUST SETTINGS TO WORK WITH LEVEL LOADER
    public void SettingsButtonPressed()
    {
        LevelLoadManager._Instance.LoadMenuOverlay(LevelLoadManager._Instance.LevelNamesList[1]);
    }

    public void CreditsButtonPressed()
    {
        LevelLoadManager._Instance.LoadMenuOverlay(LevelLoadManager._Instance.LevelNamesList[2]);
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
