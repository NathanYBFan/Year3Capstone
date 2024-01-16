using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModifierDisplay : MonoBehaviour
{
    // Serialze Fields
    [SerializeField, ReadOnly]
    [Foldout("Dependencies"), Tooltip("")]
    private Modifier modifier;

    [SerializeField, Required]
    [Foldout("Dependencies"), Tooltip("")]
    private Image modifierImage;

    [SerializeField, Required]
    [Foldout("Dependencies"), Tooltip("")]
    private TextMeshProUGUI modifierText;

    public void ResetModifier(Modifier newModifier)
    {
        modifier = newModifier;
        modifierText.text = modifier.ModifierDescription;
        modifierImage = modifier.ModifierImage;
    }

    public void ModifierClicked()
    {
        //ModifierManager._Instance.PlayerToModify.AddComponent<modifier>();
    }
}
