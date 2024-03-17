using NaughtyAttributes;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Bars : MonoBehaviour
{
	#region Serialize Fields
	[SerializeField]
    [Tooltip("Player prefab go here")]
    private PlayerStats playerStats;

    [SerializeField]
    [Foldout("UI Bars"), Tooltip("Drag in the health bar here")]
    private Image healthBar;

    [SerializeField]
    [Foldout("UI Bars"), Tooltip("Drag in the shadow health bar here")]
    private Image healthBarShadow;

    [SerializeField]
    [Foldout("UI Bars"), Tooltip("Drag in energy bar here")]
    private Image energyBar;

    [SerializeField]
    [Foldout("UI Bars"), Tooltip("Drag in shadow energy bar here")]
    private Image energyBarShadow;


    [SerializeField]
    [Foldout("UI Bars"), Tooltip("Drag in glowy bit of charcter image here")]
    private Image characterGlow;

    [SerializeField]
    [Foldout("UI Bars"), Tooltip("Drag in dark character image (no glow) here")]
    private Image characterBG;

    [Header("Shake on damage")]

    [SerializeField]
    [Foldout("Shake Dependencies"), Tooltip("Object to shake")]
    private GameObject objectToShake;

    [SerializeField]
    [Foldout("Shake Dependencies"), Tooltip("Speed of the shake")]
    private float shakeSpeed = 40f;

    [SerializeField]
    [Foldout("Shake Dependencies"), Tooltip("Magnitude of the shake")]
    private float shakeAmount = 0.01f;

    [SerializeField]
    [Foldout("Shake Dependencies"), Tooltip("How long to shake for")]
    private float maxShakeTime = 0.5f;

    [SerializeField]
    [Foldout("Shake Dependencies"), Tooltip("How long to shake for")]
    private Color characterBGDamageColor = Color.red;

    [SerializeField]
    [Foldout("Shake Dependencies"), Tooltip("How long to shake for")]
    private Color characterGlowDamageColor = Color.red;
    #endregion

    #region Getters&Setters
    public Sprite CharacterGlow { set {  characterGlow.sprite = value; } }
    public Sprite CharacterBG { set { characterBG.sprite = value; } }

    public Color CharacterGlowColour { set { characterGlow.color = value; } }
    #endregion

    private Coroutine runningShakeCoroutine;
    private Color originalCharacterBGColor;
    private Color originalCharacterGlowColor;

    private void Start()
    {
        originalCharacterBGColor = Color.white;
        originalCharacterGlowColor = playerStats.UIColor;
        FullReset();
    }

    private void OnDisable()
    {
        FullReset();
    }

    public void SetHUDBarCharacter()
    {
		characterGlow.color = playerStats.UIColor;
	}

    public void TakeDamage(int currentHealth, int previousHealth)
    {
        healthBar.fillAmount = (float)playerStats.CurrentHealth/(float)playerStats.MaxHealth;
        shakeObject(currentHealth, previousHealth);
    }

    public void UseEnergy()
    {
        energyBar.fillAmount = (float) playerStats.CurrentEnergy /(float)playerStats.MaxEnergy;
    }

    public void UpdateEnergyBar()
    {
        energyBar.fillAmount = (float)playerStats.CurrentEnergy / (float)playerStats.MaxEnergy;
    }

    public void FullReset()
    {
        characterBG.color = originalCharacterBGColor;
        characterGlow.color = originalCharacterGlowColor;
        healthBar.fillAmount = (float)playerStats.CurrentHealth / (float)playerStats.MaxHealth;
        healthBarShadow.fillAmount = (float)playerStats.CurrentHealth / (float)playerStats.MaxHealth;
    }

    private void shakeObject(int currentHealth, int previousHealth)
    {
        if (runningShakeCoroutine != null)
        {
            StopCoroutine(runningShakeCoroutine);
        }
        runningShakeCoroutine = StartCoroutine(shake(currentHealth, previousHealth));
    }

    private IEnumerator shake(int currentHealth, int previousHealth)
    {
        float timer = 0;
        var originalPosition = objectToShake.transform.position;
        var originalHealth = (float) previousHealth / (float) playerStats.MaxHealth;

        characterBG.color = characterBGDamageColor;
        characterGlow.color = characterGlowDamageColor;

        while (timer < maxShakeTime)
        {
            healthBarShadow.fillAmount = Mathf.Lerp(originalHealth, healthBar.fillAmount, timer * 2);
            timer += Time.deltaTime;
            objectToShake.transform.position = new Vector3(objectToShake.transform.position.x + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount + Random.Range(-0.2f, 0.2f), 
                objectToShake.transform.position.y + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount + Random.Range(-0.1f, 0.1f), objectToShake.transform.position.z);
            yield return null;
        }
        
        healthBarShadow.fillAmount = healthBar.fillAmount;

        characterBG.color = originalCharacterBGColor;
        characterGlow.color = originalCharacterGlowColor;
        objectToShake.transform.position = originalPosition;
    }
}
