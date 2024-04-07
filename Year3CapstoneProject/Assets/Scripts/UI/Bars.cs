using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Bars : MonoBehaviour
{
	#region SerializeFields
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
    public bool EarlyOut { get { return earlyOut; } set { earlyOut = value; } }
    #endregion

    #region PrivateVariables
    private Coroutine runningShakeCoroutine;
    private Coroutine runningAggravatedCoroutine;
    private Color originalCharacterBGColor;
    private Color originalCharacterGlowColor;
    private bool earlyOut = false;
    #endregion

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

	private void OnEnable()
	{
		FullReset();
	}

	public void SetHUDBarCharacter()
    {
		originalCharacterBGColor = Color.white;
		originalCharacterGlowColor = playerStats.UIColor;
		characterGlow.color = playerStats.UIColor;
	}

    public void TakeDamage(float currentHealth, float previousHealth)
    {
        healthBar.fillAmount = (float)playerStats.CurrentHealth/(float)playerStats.MaxHealth;
        shakeObject(currentHealth, previousHealth);
    }

	public void Heal(int currentHealth, int previousHealth)
	{
		healthBar.fillAmount = (float)playerStats.CurrentHealth / (float)playerStats.MaxHealth;
		if (runningAggravatedCoroutine != null)
		{
			StopCoroutine(runningAggravatedCoroutine);
		}
		runningAggravatedCoroutine = StartCoroutine(AggravatedHealthUpdate(currentHealth, previousHealth));
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
        characterBG.color = Color.white;
		originalCharacterGlowColor = playerStats.UIColor;
		characterGlow.color = playerStats.UIColor;
        healthBar.fillAmount = (float)playerStats.CurrentHealth / (float)playerStats.MaxHealth;
        healthBarShadow.fillAmount = (float)playerStats.CurrentHealth / (float)playerStats.MaxHealth;
    }

    private void shakeObject(float currentHealth, float previousHealth)
    {
        if (runningShakeCoroutine != null)
        {
            StopCoroutine(runningShakeCoroutine);
		}
		if (runningAggravatedCoroutine != null)
		{
			StopCoroutine(runningAggravatedCoroutine);
		}
		runningShakeCoroutine = StartCoroutine(shake(currentHealth, previousHealth));
        runningAggravatedCoroutine = StartCoroutine(AggravatedHealthUpdate(currentHealth, previousHealth));
	}

    private IEnumerator AggravatedHealthUpdate(float currentHealth, float previousHealth)
    {
		float timer = 0;
		var originalHealth = (float)previousHealth / (float)playerStats.MaxHealth;
        characterGlow.sprite = playerStats.CharacterStat.characterHurtSprite;

        while (timer < maxShakeTime)
		{
			healthBarShadow.fillAmount = Mathf.Lerp(originalHealth, healthBar.fillAmount, timer * 2);
			timer += Time.deltaTime;
			yield return null;
		}
		healthBarShadow.fillAmount = healthBar.fillAmount;
        characterGlow.sprite = playerStats.CharacterStat.characterSprite;
	}
    
    private IEnumerator shake(float currentHealth, float previousHealth)
    {
        float timer = 0;
        var originalPosition = objectToShake.transform.position;

        characterBG.color = characterBGDamageColor;
        characterGlow.color = characterGlowDamageColor;

        while (timer < maxShakeTime)
        {
            if (earlyOut) break;
            timer += Time.deltaTime;
            objectToShake.transform.position = new Vector3(objectToShake.transform.position.x + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount + Random.Range(-0.2f, 0.2f), 
                objectToShake.transform.position.y + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount + Random.Range(-0.1f, 0.1f), objectToShake.transform.position.z);
            yield return null;
        }

        characterBG.color = originalCharacterBGColor;
        characterGlow.color = originalCharacterGlowColor;
        objectToShake.transform.position = originalPosition;
    }
}
