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
    private TextMeshProUGUI modifierDescriptionText;

    [SerializeField, Required]
    [Foldout("Dependencies"), Tooltip("")]
    private TextMeshProUGUI modifierNameText;

    [SerializeField, ReadOnly]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject playerToModify;

    public void ResetModifier(Modifier newModifier, GameObject newPlayerToModify)
    {
        // Assign values
        modifier = newModifier;
        playerToModify = newPlayerToModify;

        // Display proper data
        modifierImage = modifier.modifierImage;
        modifierDescriptionText.text = modifier.modifierDescription;
        modifierNameText.text = modifier.modifierName;
    }

    public void ModifierClicked()
    {
        ModifierManager._Instance.PlayerToModify = playerToModify;
		ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>().ActivateEffects(modifier);
    }
}
