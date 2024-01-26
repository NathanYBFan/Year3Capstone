using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Singleton Initialization
    public static PlayerManager _Instance;

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Debug.LogWarning("Destroyed a repeated PlayerManager");
            Destroy(gameObject);
        }

        else if (_Instance == null)
            _Instance = this;
    }
}
