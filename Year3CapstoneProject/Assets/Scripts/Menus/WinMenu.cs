using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject firstButton;

    [SerializeField]
    private TextMeshProUGUI textbox;

    [SerializeField]
    private Image playerIcon;

    [SerializeField]
    private Image playerBgIcon;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
        textbox.text = "Player " + (GameManager._Instance.PlayerWinnerIndex + 1) + " Wins!";
        playerIcon.sprite = GameManager._Instance.Players[GameManager._Instance.PlayerWinnerIndex].GetComponent<PlayerStats>().CharacterStat.characterSprite;

        playerBgIcon.sprite = GameManager._Instance.Players[GameManager._Instance.PlayerWinnerIndex].GetComponent<PlayerStats>().CharacterStat.characterBGSprite;
    }
    private void ButtonPressSFX()
    {
        AudioSource buttonAudioSource = AudioManager._Instance.UIAudioSource;
        AudioManager._Instance.PlaySoundFX(AudioManager._Instance.UIAudioList[1], buttonAudioSource);
    }

    public void ContinuePressed()
    {
        ButtonPressSFX();
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[0], true);
    }
}
