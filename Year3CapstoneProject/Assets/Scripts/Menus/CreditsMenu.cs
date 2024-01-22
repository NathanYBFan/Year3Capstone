using UnityEngine;

public class CreditsMenu : MonoBehaviour
{
    public void BackButtonPressed()
    {
        LevelLoadManager._Instance.UnloadMenuOverlay(LevelLoadManager._Instance.LevelNamesList[2]);
    }
}
