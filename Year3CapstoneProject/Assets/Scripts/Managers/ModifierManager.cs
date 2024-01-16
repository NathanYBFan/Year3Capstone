using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class ModifierManager : MonoBehaviour
{
    // Singleton Initialization
    public static ModifierManager _Instance;

    // Serialize Fields
    [SerializeField, Required]
    [Foldout("Dependencies"), Tooltip("")]
    private GameObject modifierMenu;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private List<GameObject> listOfModifiers; // TODO NATHANF: Change scriptable Object to created script

    // Getters
    public List<GameObject> ListOfModifiers { get { return listOfModifiers; } }

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Debug.LogWarning("Destroyed a repeated ModifierManager");
            Destroy(this.gameObject);
        }
        else if (_Instance == null)
            _Instance = this;
    }

    public void OpenModifierMenu()
    {
        Time.timeScale = 0;
        modifierMenu.SetActive(true);
    }

    public void CloseModifierMenu()
    {
        Time.timeScale = 1;
        modifierMenu.SetActive(false);
    }
}
