using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LastGameResultsDisplay : MonoBehaviour
{
    [SerializeField]
    private Image bgImage; // assign color

    [SerializeField]
    private TextMeshProUGUI score;

    [SerializeField]
    private Image characterBGImage;

    [SerializeField]
    private Image characterFaceImage;

    [SerializeField]
    private int playerIndex;

    private void OnEnable()
    {
        if (LastGameResults._Instance.CharacterUIColors.Count == 0)
            return;

        Color temp = LastGameResults._Instance.CharacterUIColors[LastGameResults._Instance.WinOrder[playerIndex]];
        characterFaceImage.color = temp;

        temp.a = temp.a / 2;
        bgImage.color = temp;
     
        score.text = LastGameResults._Instance.Scores[LastGameResults._Instance.WinOrder[playerIndex]].ToString();
        characterBGImage.sprite = LastGameResults._Instance.PlayerUIBG[LastGameResults._Instance.WinOrder[playerIndex]];
        characterFaceImage.sprite = LastGameResults._Instance.PlayerUIFace[LastGameResults._Instance.WinOrder[playerIndex]];
    }
}
