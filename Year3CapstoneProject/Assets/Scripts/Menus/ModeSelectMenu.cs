using NaughtyAttributes;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ModeSelectMenu : MonoBehaviour
{
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("First button to be selected - for controllers")]
    private GameObject firstButton;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private string[] modesToSelectFrom;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private TextMeshProUGUI modeTextDisplay;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private int currentSelectedMode = 0;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
        for (int i = 0; i < modesToSelectFrom.Length; i++)
        {
            if (GameManager._Instance.SelectedGameMode.CompareTo(modesToSelectFrom[i]) == 0)
            {
                currentSelectedMode = i;
                modeTextDisplay.text = modesToSelectFrom[currentSelectedMode];
                return;
            }
        }
        modeTextDisplay.text = modesToSelectFrom[currentSelectedMode];
    }

    public void LeftArrowPressed()
    {
        currentSelectedMode -= 1;
        if (currentSelectedMode < 0)
            currentSelectedMode = modesToSelectFrom.Length - 1;
        modeTextDisplay.text = modesToSelectFrom[currentSelectedMode];
    }

    public void RightArrowPressed()
    {
        currentSelectedMode += 1;
        if (currentSelectedMode > modesToSelectFrom.Length - 1)
            currentSelectedMode = 0;
        modeTextDisplay.text = modesToSelectFrom[currentSelectedMode];
    }

    public void ContinueButtonPressed()
    {
        ButtonPressSFX();
        GameManager._Instance.SelectedGameMode = modesToSelectFrom[currentSelectedMode];
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[4], true);
    }

    public void BackButtonPressed()
    {
        ButtonPressSFX();
        GameManager._Instance.SelectedGameMode = modesToSelectFrom[currentSelectedMode];
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[0], false);
    }


    //finds the UIAudioSource, and plays the button press sound
    public void ButtonPressSFX()
    {
        AudioSource buttonAudioSource = AudioManager._Instance.UIAudioSource;
        AudioManager._Instance.PlaySoundFX(AudioManager._Instance.UIAudioList[1], buttonAudioSource);
    }

}
