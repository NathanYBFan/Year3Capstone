using NaughtyAttributes;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    // Singleton Initialization
    public static PointsManager _Instance;

    #region SerializeFields
    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("")]
    private int[] pointsPerPlayer;
    #endregion

    #region PrivateVariables
    private int winThreshold;
    #endregion

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Debug.LogWarning("Destroyed a repeated PointsManager");
            Destroy(this.gameObject);
        }

        else if (_Instance == null)
            _Instance = this;
    }

    private void Start()
    {
        // Make number of points to record equal to the number of players
        pointsPerPlayer = new int[GameManager._Instance.Players.Count];
    }
    
    // Increase a select amount of points for a selected player
    public void IncreasePoints(int playerToIncrease, int amountOfPointsToAdd)
    {
        pointsPerPlayer[playerToIncrease] += amountOfPointsToAdd;
        CheckWinCondition();
    }

    // Check to see if any player has met the win condition
    private void CheckWinCondition()
    {
        for (int i = 0; i < pointsPerPlayer.Length; i++)
        {
            if (pointsPerPlayer[i] >= winThreshold)
            {
                pointsPerPlayer[i] = winThreshold;
                GameManager._Instance.WinConditionMet();
            }
        }
    }
}
