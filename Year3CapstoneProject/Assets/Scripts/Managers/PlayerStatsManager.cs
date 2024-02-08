using NaughtyAttributes;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    // Singleton Initialization
    public static PlayerStatsManager _Instance;

    #region SerializeFields
    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("Array of points for each player")]
    private int[] playerPoints = new int[GameManager._Instance.Players.Count];

    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("Array of kills each player has done")]
    private int[] playerKills = new int[GameManager._Instance.Players.Count];

    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("Array of kills each player has done")]
    private int[] playerDeaths = new int[GameManager._Instance.Players.Count];
    #endregion

    #region Setters&Getters
    public int[] PlayerPoints { get { return playerPoints; } set { playerPoints = value; } }
    public int[] PlayerKills { get { return playerKills; } set { playerKills = value; } }
    public int[] PlayerDeaths { get { return playerDeaths; } set { playerDeaths = value; } }
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
    }

    public void IncreaseKillCounter(int playerNumber, int amount)
    {
        playerKills[playerNumber] += amount;
    }

    public void IncreaseDeathCounter(int playerNumber, int amount)
    {
        playerDeaths[playerNumber] += amount;
    }
}
