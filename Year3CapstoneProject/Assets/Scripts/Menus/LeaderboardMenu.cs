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
    

}
