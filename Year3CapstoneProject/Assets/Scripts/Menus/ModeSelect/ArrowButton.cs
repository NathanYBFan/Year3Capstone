using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowButton : MonoBehaviour, ISelectHandler
{
    [SerializeField]
    private GameObject continueButton;

    [SerializeField]
    private bool isPointingLeft;

    [SerializeField]
    private ModeSelectMenu modeSelectMenu;

    public void OnSelect(BaseEventData eventData)
    {
        if (isPointingLeft)
            modeSelectMenu.LeftArrowPressed();
        else
            modeSelectMenu.RightArrowPressed();
        StartCoroutine(SetNewButton());
    }

    private IEnumerator SetNewButton()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(continueButton);
    }
}
