using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class Bars : MonoBehaviour
{
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

    private void Start()
    {
        characterGlow.sprite = playerStats.CharacterStat.characterSprite;
        characterGlow.color = playerStats.UIColor;
        characterBG.sprite = playerStats.CharacterStat.characterBGSprite;
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
