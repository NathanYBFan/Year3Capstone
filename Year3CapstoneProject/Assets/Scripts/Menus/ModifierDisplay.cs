using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModifierDisplay : MonoBehaviour
{
    // Serialze Fields
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private Modifier modifier;

    [SerializeField, Required]
    [Foldout("Dependencies"), Tooltip("")]
    private Image modifierImage;

    [SerializeField, Required]
    [Foldout("Dependencies"), Tooltip("")]
    private TextMeshProUGUI modifierText;

    public GameObject playerToModify;

    public void ResetModifier(Modifier newModifier)
    {
        modifier = newModifier;
    }

    public void ModifierClicked()
    {
        ModifierManager._Instance.PlayerToModify = playerToModify;
		ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>().ActivateEffects(modifier);
        
    }
}
