using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    [Foldout("Leadboard Bars"), Tooltip("Drag in leaderboard bars here")]
    private Image leaderboardBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    //private void GivePoints()
    //{
    //    PlayerStatsManager.Instance.
    //}
 
}
