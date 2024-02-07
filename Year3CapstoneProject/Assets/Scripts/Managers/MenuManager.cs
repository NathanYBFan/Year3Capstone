using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Singleton Initialization
    public static MenuManager _Instance;

    #region SerializeFields
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private List<GameObject> playerInputs = new List<GameObject>();
    #endregion

    #region Getters&Setters
    public List<GameObject> PlayerInputs { get { return playerInputs; } set { playerInputs = value; } }
    #endregion

    private void Awake()
    {
        if (_Instance != null && _Instance != this) // TODO NATHANF: FIND A USE OR REMOVE
        {
            Debug.LogWarning("Destroyed a repeated MenuManager");
            Destroy(gameObject);
        }

        else if (_Instance == null)
            _Instance = this;
    }
}
