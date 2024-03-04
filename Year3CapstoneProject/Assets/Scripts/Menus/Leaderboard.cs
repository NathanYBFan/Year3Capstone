using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    //[SerializeField]
    //[Foldout("Leadboard Bars"), Tooltip("Drag in leaderboard bars here")]
    //private Image leaderboardBar;
    [SerializeField]
    private int playerNum;

    [SerializeField]
    Slider pointSlider;

    [SerializeField]
    Image fill;


    [SerializeField]
    TextMeshProUGUI text;


    private float maxPoints;
    private GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        maxPoints = 49;
        player = GameManager._Instance.Players[playerNum];
        fill.color = player.GetComponent<PlayerStats>().UIColor;
    }


    private void OnEnable()
    {
        text.text = player.name + ": " + PlayerStatsManager._Instance.playerPoints[playerNum];
        fill.color = player.GetComponent<PlayerStats>().UIColor;
        pointSlider.value = PlayerStatsManager._Instance.playerPoints[playerNum] / maxPoints;

    }

}
