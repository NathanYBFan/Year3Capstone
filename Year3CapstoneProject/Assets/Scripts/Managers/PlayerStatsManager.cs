using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    // Singleton Initialization
    public static PlayerStatsManager _Instance;

    #region SerializeFields
    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("Array of points for each player")]
    private int[] playerPoints;

    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("Array of kills each player has done")]
    private int[] playerKills;

    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("Array of kills each player has done")]
    private int[] playerDeaths;

    [SerializeField]
    [Foldout("Stats"), Tooltip("Number of points needed to Win")]
    private int pointsThreashold = 20;

    [SerializeField]
    [Foldout("Stats"), Tooltip("Number of points to assign for order First place = 0")]
    private int[] pointsToGiveForPosition;
    #endregion

    #region Setters&Getters
    public int[] PlayerPoints { get { return playerPoints; } set { playerPoints = value; } }
    public int[] PlayerKills { get { return playerKills; } set { playerKills = value; } }
    public int[] PlayerDeaths { get { return playerDeaths; } set { playerDeaths = value; } }
    public int[] PointsToGiveForPosition { get { return pointsToGiveForPosition; } set { pointsToGiveForPosition = value; } }
    //public int[] PlayerFourPoints { get { return playerFourPoints; } set {  playerFourPoints = value; } }
    #endregion

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Debug.LogWarning("Destroyed a repeated PlayerStatsManager");
            Destroy(this.gameObject);
        }

        else if (_Instance == null)
            _Instance = this;
    }

    private void Start()
    {
        playerPoints = new int[GameManager._Instance.Players.Count];
        playerKills = new int[GameManager._Instance.Players.Count];
        playerDeaths = new int[GameManager._Instance.Players.Count];
    }
    public void ResetStats()
    {
        for (int i = 0; i < GameManager._Instance.Players.Count; i++)
        {
            // Reset all points
            playerPoints[i] = 0;
            // Reset kills
            playerKills[i] = 0;
            // Reset Deaths
            playerDeaths[i] = 0;
        }
    }

    public void IncreasePoints(int playerNumber, int amount)
    {
        playerPoints[playerNumber] += amount;
        CheckWinCondition();
    }

    public void IncreaseKillCounter(int playerNumber, int amount)
    {
        playerKills[playerNumber] += amount;
    }

    public void IncreaseDeathCounter(int playerNumber, int amount)
    {
        playerDeaths[playerNumber] += amount;
    }

    // Check to see if any player has met the win condition
    private void CheckWinCondition()
    {
        for (int i = 0; i < PlayerPoints.Length; i++)
        {
            if (PlayerPoints[i] >= pointsThreashold)
                WinConditionMet();
        }
    }

    private void WinConditionMet()
    {
        // Make local variables
        List<int> playerWinOrder = new List<int>();
        List<int> localPoints = new List<int>();
        
        // Fill local variables
        for (int i = 0; i < playerPoints.Length; i++)
            localPoints.Add(playerPoints[i]);

        // For the number of players there are
        for (int j = 0; j < playerPoints.Length; j++)
        {
            int max = 0; // Max number
            int index = 0;

            for (int i = 0; i < localPoints.Count; i++) // Check the points list
            {
                if (localPoints[i] > max)
                {
                    max = localPoints[i]; // Get max number
                    index = i;
                    localPoints[i] = -1;
                }
            }

            playerWinOrder.Add(index);
        }

        // Pass win order to win screens
        GameManager._Instance.WinConditionMet(playerWinOrder);
    }
}
