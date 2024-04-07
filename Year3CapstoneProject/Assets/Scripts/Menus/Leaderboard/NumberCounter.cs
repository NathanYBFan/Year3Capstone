using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using TMPro;
using UnityEngine;

public class NumberCounter : MonoBehaviour
{
    #region SerializeFields
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private TextMeshProUGUI textBox;
    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private int countFPS = 30;
    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private float duration = 1f;
    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private string numberFormat = "N0";
    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private string textPrefix;
    #endregion

    #region PrivateVariables
    private int _value;
    private Coroutine CountingCoroutine;
    #endregion

    #region Getters&Setters
    public int Value { get { return _value; } set { UpdateText(value); _value = value; } }
    #endregion

    private void UpdateText(int newValue) 
    {
        if (CountingCoroutine != null) StopCoroutine(CountingCoroutine);

        //CountingCoroutine = StartCoroutine(CountText(newValue));
        textBox.text = textPrefix + newValue.ToString(numberFormat);
    }

    private IEnumerator CountText(int newValue)
    {
        WaitForSeconds wait = new WaitForSeconds(1f / countFPS);
        int previousValue = _value;
        int stepAmount;

        if (newValue - previousValue < 0)
            stepAmount = Mathf.FloorToInt((newValue - previousValue) / (countFPS * duration));
        else
            stepAmount = Mathf.CeilToInt((newValue - previousValue) / (countFPS * duration));

        if (previousValue < newValue)
        {
            while (previousValue < newValue)
            {
                previousValue += stepAmount;
                if (previousValue > newValue)
                    previousValue = newValue;

                textBox.SetText(textPrefix + previousValue.ToString(numberFormat));

                yield return wait;
            }
        }
        else
        {
            while (previousValue > newValue)
            {
                previousValue += stepAmount;
                if (previousValue < newValue)
                    previousValue = newValue;

                textBox.SetText(textPrefix + previousValue.ToString(numberFormat));

                yield return wait;
            }
        }
    }
}
