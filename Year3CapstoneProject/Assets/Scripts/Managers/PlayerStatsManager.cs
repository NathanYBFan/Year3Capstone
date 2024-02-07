using NaughtyAttributes;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    // Singleton Initialization
    public static PlayerStatsManager _Instance; // TODO NATHANF: THIS IS TO KEEP TRACK OF KILLS AND OTHER STATS

    #region SerializeFields
    [SerializeField]
    [Foldout(""), Tooltip("")]
    private int playerOnePoints;

    [SerializeField]
    [Foldout(""), Tooltip("")]
    private int playerTwoPoints;

    [SerializeField]
    [Foldout(""), Tooltip("")]
    private int playerThreePoints;

    [SerializeField]
    [Foldout(""), Tooltip("")]
    private int playerFourPoints;
    #endregion

    #region Setters&Getters
    public int PlayerOnePoints { get { return playerOnePoints; } set { playerOnePoints = value; } }
    public int PlayerTwoPoints { get { return playerTwoPoints; } set {  playerTwoPoints = value; } }
    public int PlayerThreePoints { get { return playerThreePoints; } set { playerThreePoints = value; } }
    public int PlayerFourPoints { get { return playerFourPoints; } set {  playerFourPoints = value; } }
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
}
