using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModifierDisplay : MonoBehaviour
{
    #region SerializeFields
    [SerializeField, ReadOnly]
    [Foldout("Dependencies"), Tooltip("")]
    private Modifier modifier;

    [SerializeField, Required]
    [Foldout("Dependencies"), Tooltip("")]
    private Image modifierImageFirstLayer;

    [SerializeField, Required]
    [Foldout("Dependencies"), Tooltip("")]
    private Image modifierImageSecondLayer;

    [SerializeField, Required]
    [Foldout("Dependencies"), Tooltip("")]
    private Image modifierImageThirdLayer;

    [SerializeField, Required]
    [Foldout("Dependencies"), Tooltip("")]
    private Image modifierImageFourthLayer;

    [SerializeField, Required]
    [Foldout("Dependencies"), Tooltip("")]
    private TextMeshProUGUI modifierDescriptionText;

    [SerializeField, Required]
    [Foldout("Dependencies"), Tooltip("")]
    private TextMeshProUGUI modifierNameText;
    
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject buttonObject;

    [SerializeField, ReadOnly]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject playerToModify;
    #endregion

    #region Setters&Getters
    public GameObject Buttonobject { get { return buttonObject; } }
	#endregion

	public void UpdateColour()
	{
		if (modifierImageFirstLayer.sprite.name.Contains("Colour")) modifierImageFirstLayer.color =
		ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>().UIColor;
        else modifierImageFirstLayer.color = Color.white;

		if (modifierImageSecondLayer.sprite.name.Contains("Colour")) modifierImageSecondLayer.color =
		ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>().UIColor;
		else modifierImageSecondLayer.color = Color.white;

		if (modifierImageThirdLayer.sprite.name.Contains("Colour")) modifierImageThirdLayer.color =
		ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>().UIColor;
		else modifierImageThirdLayer.color = Color.white;

		if (modifierImageFourthLayer.sprite.name.Contains("Colour")) modifierImageFourthLayer.color =
		ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>().UIColor;
		else modifierImageFourthLayer.color = Color.white;
	}

	public void ResetModifier(Modifier newModifier, GameObject newPlayerToModify)
    {
        // Assign values
        modifier = newModifier;
        playerToModify = newPlayerToModify;

        // Display proper data
        modifierImageFirstLayer.sprite = modifier.modifierImageFirstLayer;
        modifierImageSecondLayer.sprite = modifier.modifierImageSecondLayer;
        modifierImageThirdLayer.sprite = modifier.modifierImageThirdLayer;
        modifierImageFourthLayer.sprite = modifier.modifierImageFourthLayer;
        modifierImageFirstLayer.preserveAspect = true;
        modifierImageSecondLayer.preserveAspect = true;
        modifierImageThirdLayer.preserveAspect = true;
        modifierImageFourthLayer.preserveAspect = true;
        modifierDescriptionText.text = modifier.modifierDescription;
        modifierNameText.text = modifier.modifierName;
    }

    public void ModifierClicked()
    {
        ModifierManager._Instance.PlayerToModify = playerToModify;
		ModifierManager._Instance.PlayerToModify.GetComponent<PlayerStats>().ActivateEffects(modifier);
        
        ModifierManager._Instance.CloseModifierMenu();
    }
}
