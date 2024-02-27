using NaughtyAttributes;
using UnityEngine;

public abstract class MenuNavigation : MonoBehaviour
{
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Array of buttons in this menu")]
    public GameObject[] arrayOfbuttons;
    public abstract void UpPressed();
    public abstract void DownPressed();
    public abstract void LeftPressed();
    public abstract void RightPressed();
    public abstract void SelectPressed(int buttonSelection);
    public abstract void CancelPressed();
    public abstract void UpdateUI(GameObject buttonSelection);
}
