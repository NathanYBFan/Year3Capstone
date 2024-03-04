using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private GameObject firstButton;

    [SerializeField]
    private int playerNum;

    [SerializeField]
    Slider pointSlider;

    [SerializeField]
    Image fill;

    [SerializeField]
    TextMeshProUGUI playerName;

    [SerializeField]
    TextMeshProUGUI scoreText;

    private float maxPoints;
    private GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        maxPoints = 60;
        player = GameManager._Instance.Players[playerNum];
        fill.color = player.GetComponent<PlayerStats>().UIColor;
    }


    private void Update()
    {
        if (Input.anyKeyDown)
            NextButtonPressed();
    }
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
        playerName.text = player.name;
        scoreText.text = ""+PlayerStatsManager._Instance.playerPoints[playerNum];
        fill.color = player.GetComponent<PlayerStats>().UIColor;
        pointSlider.value = PlayerStatsManager._Instance.playerPoints[playerNum] / maxPoints;

        if (pointSlider.value > 0.1f)
        {
            scoreText.gameObject.transform.position = new Vector3(scoreText.gameObject.transform.position.x, scoreText.gameObject.transform.position.y - 70, scoreText.gameObject.transform.position.z);
        }
    }

    public void NextButtonPressed()
    {
        ModifierManager._Instance.CloseLeaderBoardMenu();
    }
}
