using NaughtyAttributes;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    [Foldout("Dependencies")]
    private GameObject firstButton;

    [SerializeField]
    [Foldout("Dependencies")]
    private int playerNum;

    [SerializeField]
    [Foldout("Dependencies")]
    private Image barBg;

    [SerializeField]
    [Foldout("Dependencies")]
    private Image crown;

    [SerializeField]
    [Foldout("Dependencies")]
    private Image gems;

    [SerializeField]
    [Foldout("Dependencies")]
    private Image playerIcon;

    [SerializeField]
    [Foldout("Dependencies")]
    private Image playerIconBg;

    [SerializeField]
    [Foldout("Dependencies")]
    private TextMeshProUGUI playerName;

    [SerializeField]
    [Foldout("Dependencies")]
    private TextMeshProUGUI currPoints;

    [SerializeField]
    [Foldout("Dependencies")]
    private TextMeshProUGUI pointGain;

    private float maxPoints;
    private GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        maxPoints = 60;
        player = GameManager._Instance.Players[playerNum];
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
        playerName.text = player.name;
        currPoints.text = ""+PlayerStatsManager._Instance.playerPoints[playerNum];
    }

    public void NextButtonPressed()
    {
        ModifierManager._Instance.CloseLeaderBoardMenu();
    }
}
