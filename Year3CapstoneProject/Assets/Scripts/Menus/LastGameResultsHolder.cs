using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastGameResultsHolder : MonoBehaviour
{
    private void OnEnable()
    {
        if (LastGameResults._Instance.CharacterUIColors.Count == 0)
            gameObject.SetActive(false);
    }
}
