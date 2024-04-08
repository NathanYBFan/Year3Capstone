using NaughtyAttributes;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinPlacements : MonoBehaviour
{
    [SerializeField]
    [Foldout("Dependencies")]
    private GameObject crownRootObject;

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
    private Image numberImage;

    [SerializeField]
    [Foldout("Dependencies")]
    private Image numberBgHighlight;

    [SerializeField]
    [Foldout("Dependencies")]
    private TextMeshProUGUI playerName;

    [SerializeField]
    [Foldout("Dependencies")]
    private TextMeshProUGUI currPoints;

    [SerializeField]
    [Foldout("Dependencies")]
    private Sprite[] playerPlacementText;

    [SerializeField]
    [Foldout("Dependencies")]
    private Sprite[] playerPlacementBGText;


    private void OnEnable()
    {
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

        crownRootObject.SetActive(false);
        if (playerNum == 0 || PlayerStatsManager._Instance.PlayerPoints[playerWinOrder[0]] == PlayerStatsManager._Instance.PlayerPoints[playerWinOrder[playerNum]])
        {
            crownRootObject.SetActive(true);
            numberImage.sprite = playerPlacementText[0];
            numberBgHighlight.sprite = playerPlacementBGText[0];
        }
        else if (PlayerStatsManager._Instance.PlayerPoints[playerWinOrder[1]] == PlayerStatsManager._Instance.PlayerPoints[playerWinOrder[playerNum]])
        {
            numberImage.sprite = playerPlacementText[1];
            numberBgHighlight.sprite = playerPlacementBGText[1];
        }
        else if (PlayerStatsManager._Instance.PlayerPoints[playerWinOrder[2]] == PlayerStatsManager._Instance.PlayerPoints[playerWinOrder[playerNum]])
        {
            // 1 == 2
            if (PlayerStatsManager._Instance.PlayerPoints[playerWinOrder[1]] == PlayerStatsManager._Instance.PlayerPoints[playerWinOrder[0]])
            {
                numberImage.sprite = playerPlacementText[1];
                numberBgHighlight.sprite = playerPlacementBGText[1];
            }
            else
            {
                numberImage.sprite = playerPlacementText[2];
                numberBgHighlight.sprite = playerPlacementBGText[2];
            }
            
        }
        else if (PlayerStatsManager._Instance.PlayerPoints[playerWinOrder[3]] == PlayerStatsManager._Instance.PlayerPoints[playerWinOrder[playerNum]])
        {
            // 1 == 2
            if (PlayerStatsManager._Instance.PlayerPoints[playerWinOrder[0]] == PlayerStatsManager._Instance.PlayerPoints[playerWinOrder[1]])
            {
                // 1 == 2 == 3 != 4
                if (PlayerStatsManager._Instance.PlayerPoints[playerWinOrder[0]] == PlayerStatsManager._Instance.PlayerPoints[playerWinOrder[2]])
                {
                    numberImage.sprite = playerPlacementText[1];
                    numberBgHighlight.sprite = playerPlacementBGText[1];
                }
                // 1 == 2 != 3 != 4
                else
                {
                    numberImage.sprite = playerPlacementText[2];
                    numberBgHighlight.sprite = playerPlacementBGText[2];
                }
            }
            else
            {
                numberImage.sprite = playerPlacementText[3];
                numberBgHighlight.sprite = playerPlacementBGText[3];
            }
        }

        int selectedNumber = playerWinOrder[playerNum];
        CharacterStatsSO characterStats = GameManager._Instance.Players[selectedNumber].GetComponent<PlayerStats>().CharacterStat;

        // Set text boxes
        playerName.text = "Player " + (selectedNumber + 1).ToString();
        currPoints.text = PlayerStatsManager._Instance.PlayerPoints[selectedNumber].ToString();

        // Set sprites
        playerIcon.sprite = characterStats.characterBGSprite;
        playerIconBg.sprite = characterStats.characterSprite;

        Color colorToApply = GameManager._Instance.Players[selectedNumber].GetComponent<PlayerStats>().UIColor;

        gems.color = colorToApply;
        barBg.color = colorToApply;
        playerIconBg.color = colorToApply;
        numberBgHighlight.color = colorToApply;
    }

    public void NextButtonPressed()
    {
        ModifierManager._Instance.CloseLeaderBoardMenu();
    }
}
