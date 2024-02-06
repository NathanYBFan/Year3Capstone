using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    // Singleton Initialization
    public static MenuManager _Instance;

    // Serialize Fields
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private List<GameObject> playerInputs = new List<GameObject>();

    // Getters & Setters
    public List<GameObject> PlayerInputs { get { return playerInputs; } set { playerInputs = value; } }

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Debug.LogWarning("Destroyed a repeated MenuManager");
            Destroy(gameObject);
        }

        else if (_Instance == null)
            _Instance = this;
    }


    public void ResetAllInputs()
    {
        Debug.Log("Reset all inputs");
    }
}
