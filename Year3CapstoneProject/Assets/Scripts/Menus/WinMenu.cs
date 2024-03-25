using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
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
        textbox.text = "Player " + (GameManager._Instance.PlayerWinnerIndex + 1) + " Wins";
        playerIcon.sprite = GameManager._Instance.Players[GameManager._Instance.PlayerWinnerIndex].GetComponent<PlayerStats>().CharacterStat.characterSprite;
        playerBgIcon.sprite = GameManager._Instance.Players[GameManager._Instance.PlayerWinnerIndex].GetComponent<PlayerStats>().CharacterStat.characterBGSprite;
        playerIcon.color = GameManager._Instance.Players[GameManager._Instance.PlayerWinnerIndex].GetComponent<PlayerStats>().UIColor;

        // Play audio
        StartCoroutine(WinScreenAudio());
	}
    private void OnDisable()
    {
        StopAllCoroutines();
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

    private IEnumerator WinScreenAudio()
    {
        AudioClip clipToPlay;
        switch(GameManager._Instance.Players[GameManager._Instance.PlayerWinnerIndex].GetComponent<PlayerStats>().CharacterStat.CharacterName)
        {
            case "Cube":
                clipToPlay = AudioManager._Instance.WinScreenCharacterNamesList[0];
                break;
            case "Octo":
                clipToPlay = AudioManager._Instance.WinScreenCharacterNamesList[1];
                break;
            case "Pyr":
                clipToPlay = AudioManager._Instance.WinScreenCharacterNamesList[2];
                break;
            case "Twelve":
                clipToPlay = AudioManager._Instance.WinScreenCharacterNamesList[3];
                break;
            default:
                clipToPlay = AudioManager._Instance.WinScreenCharacterNamesList[0];
                break;
        }
        AudioManager._Instance.PlaySoundFX(clipToPlay, AudioManager._Instance.MRTwentyAudioSource);

        // Wait until finished
        while (AudioManager._Instance.MRTwentyAudioSource.isPlaying)
            yield return null;

        clipToPlay = AudioManager._Instance.WinScreenWinMessageList[Random.Range(0,1)];
        AudioManager._Instance.PlaySoundFX(clipToPlay, AudioManager._Instance.MRTwentyAudioSource);
    }
}
