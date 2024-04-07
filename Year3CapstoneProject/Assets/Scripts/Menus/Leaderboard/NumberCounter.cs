using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
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
    #endregion

    private int _value;
    private Coroutine CountingCoroutine;


    public int Value { get { return _value; } set { UpdateText(); _value = value; } }

    private void Awake()
    {
        
    }

    private void UpdateText() 
    {
        
    }
}
