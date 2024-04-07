using NaughtyAttributes;
using System.Collections.Generic;
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

    [SerializeField]
    [Foldout("Dependencies")]
    private NumberCounter currentPointsNumberCounter;

    [SerializeField]
    [Foldout("Dependencies")]
    private NumberCounter addedPointsNumberCounter;


    private GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameManager._Instance.Players[playerNum];
    }

    private void Update()
    {
        //if (Input.anyKeyDown)
        //    ModifierManager._Instance.CloseLeaderBoardMenu();

        if (Input.GetKeyDown(KeyCode.M))
        {
            currentPointsNumberCounter.Value = PlayerStatsManager._Instance.PlayerPoints[playerNum];
        }
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
        
        List<int> playerWinOrder = new List<int>();     // Saved win order
        List<int> localPoints = new List<int>();        // Local save of the points

        for (int i = 0; i < PlayerStatsManager._Instance.PlayerPoints.Length; i++)
            localPoints.Add(PlayerStatsManager._Instance.PlayerPoints[i]);

        for (int j = 0; j < 4; j++)
        {
            int max = 0;    // Max saved number
            int index = 0;  // Index max number is found

            for (int i = 0; i < 4; i++) // Check the points list
            {
                if (localPoints[i] > max)
                {
                    max = localPoints[i]; // Get max number
                    index = i;
                }
            }

            localPoints[index] = -1;
            playerWinOrder.Add(index); // Most points to smallest
        }

        int selectedNumber = playerWinOrder[playerNum];
        CharacterStatsSO characterStats = GameManager._Instance.Players[selectedNumber].GetComponent<PlayerStats>().CharacterStat;

        // Set text boxes
        playerName.text = player.name;
        currentPointsNumberCounter.Value = PlayerStatsManager._Instance.PlayerPoints[selectedNumber];
        addedPointsNumberCounter.Value = PlayerStatsManager._Instance.PlayerPointsAddedLastRound[selectedNumber];

        // Set sprites
        playerIcon.sprite = characterStats.characterBGSprite;
        playerIconBg.sprite = characterStats.characterSprite;

        Color colorToApply = GameManager._Instance.Players[selectedNumber].GetComponent<PlayerStats>().UIColor;

        gems.color = colorToApply;
        barBg.color = colorToApply;
        playerIconBg.color = colorToApply;
    }

    public void NextButtonPressed()
    {
        ModifierManager._Instance.CloseLeaderBoardMenu();
    }
}
