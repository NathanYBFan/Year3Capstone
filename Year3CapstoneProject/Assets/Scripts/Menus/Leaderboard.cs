using NaughtyAttributes;
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
    private Image crown;

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

        //if (pointSlider.value > 0.1f)
        {
           // scoreText.gameObject.transform.position = new Vector3(scoreText.gameObject.transform.position.x, scoreText.gameObject.transform.position.y - 70, scoreText.gameObject.transform.position.z);
        }
    }

    public void NextButtonPressed()
    {
        ModifierManager._Instance.CloseLeaderBoardMenu();
    }
}
