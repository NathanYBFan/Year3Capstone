using UnityEngine;

public class DebugYoMama : MonoBehaviour
{
    void Update()
    {
        if (!GameManager._Instance.InGame) return;
        if (Input.GetKeyDown(KeyCode.Z))
            GameManager._Instance.WinConditionMet();
    }
}
