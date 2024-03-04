using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject leaderboardMenu;

    public void OpenScoreBoard()
    {
        leaderboardMenu.SetActive(true);
    }

    // Close Menu actions
    public void CloseScoreBoard()
    {
        Time.timeScale = 1;
        leaderboardMenu.SetActive(false);
        

        GameManager._Instance.StartNewGame();
    }
}
