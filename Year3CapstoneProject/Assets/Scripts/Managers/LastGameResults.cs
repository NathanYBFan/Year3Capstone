using System.Collections.Generic;
using UnityEngine;

public class LastGameResults : MonoBehaviour
{
    public static LastGameResults _Instance;

    [SerializeField]
    private List<int> winOrder;

    [SerializeField]
    private List<int> scores;

    [SerializeField]
    private List<Sprite> playerUIBG;

    [SerializeField]
    private List<Sprite> playerUIFace;

    [SerializeField]
    private List<Color> characterUIColors;

    public List<int> WinOrder { get { return winOrder; } set { winOrder = value; } }
    public List<int> Scores { get { return scores; } set { scores = value; } }

    public List<Sprite> PlayerUIFace { get { return playerUIFace; } set { playerUIFace = value; } }
    public List<Sprite> PlayerUIBG { get { return playerUIBG; } set { playerUIBG = value; } }
    public List<Color> CharacterUIColors { get { return characterUIColors; } set { characterUIColors = value; } }

    private void Awake()
	{
		if (_Instance != null && _Instance != this)
		{
			Destroy(this.gameObject);
		}

		else if (_Instance == null)
			_Instance = this;
	}

    public void ResetAll()
    {
        winOrder.Clear();
        scores.Clear();
        playerUIFace.Clear();
        playerUIBG.Clear();
        characterUIColors.Clear();
    }
}
