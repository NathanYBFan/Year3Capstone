using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardMenu : MonoBehaviour
{
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Character Stat scriptable object of stats to assign")]
    private Slider[] playerPointSliders;
    [SerializeField]
    private int playerNum;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        GetComponentInChildren<Slider>().value = PlayerStatsManager._Instance.PointsToGiveForPosition[playerNum] / 21;

    }
    private void OnEnable()
    {
    }

}
