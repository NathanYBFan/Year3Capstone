using UnityEngine;
using UnityEngine.UI;

public class Bars : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Player prefab go here")]
    private PlayerStats playerStats;

    private Image healthBar;
    private Image energyBar;
    
    private void TakeDamage()
    {
        healthBar.fillAmount = 1;
    }

    private void UseEnergy()
    {
        energyBar.fillAmount = 1;
    }
}
