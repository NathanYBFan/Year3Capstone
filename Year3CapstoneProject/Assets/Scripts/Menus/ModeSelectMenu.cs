using NaughtyAttributes;
using UnityEngine;
using TMPro;
using System.Linq;

public class ModeSelectMenu : MonoBehaviour
{
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
        GameManager._Instance.SelectedGameMode = modesToSelectFrom[currentSelectedMode];
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[4], true);
    }

    public void BackButtonPressed()
    {
        GameManager._Instance.SelectedGameMode = modesToSelectFrom[currentSelectedMode];
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[0], false);
    }
}
