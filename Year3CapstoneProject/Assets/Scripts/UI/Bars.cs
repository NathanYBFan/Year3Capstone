using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class Bars : MonoBehaviour
{
	#region Serialize Fields
	[SerializeField]
    [Tooltip("Player prefab go here")]
    private PlayerStats playerStats;

    [SerializeField]
    [Foldout("UI Bars"), Tooltip("Drag in Health bars here")]
    private Image healthBar;

    [SerializeField]
    [Foldout("UI Bars"), Tooltip("Drag in Energy bar here")]
    private Image energyBar;

    [SerializeField]
    [Foldout("UI Bars"), Tooltip("Drag in glowy bit of charcter image here")]
    private Image characterGlow;

    [SerializeField]
    [Foldout("UI Bars"), Tooltip("Drag in dark character image (no glow) here")]
    private Image characterBG;
	#endregion

    public Sprite CharacterGlow { set {  characterGlow.sprite = value; } }
    public Sprite CharacterBG { set { characterBG.sprite = value; } }

    public Color CharacterGlowColour { set { characterGlow.color = value; } }

    public void SetHUDBarCharacter()
    {
		characterGlow.color = playerStats.UIColor;
	}

    private void TakeDamage()
    {
        healthBar.fillAmount = (float)playerStats.CurrentHealth/(float)playerStats.MaxHealth;
    }

    private void UseEnergy()
    {
        energyBar.fillAmount = (float) playerStats.CurrentEnergy /(float)playerStats.MaxEnergy;
    }

    private void Update()
    {
        TakeDamage();
        UseEnergy();
    }
}
