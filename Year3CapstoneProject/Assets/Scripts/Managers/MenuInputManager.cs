using System.Collections.Generic;
using System;
using UnityEngine;

public class MenuInputManager : MonoBehaviour
{
    public static MenuInputManager _Instance;

    [SerializeField]
    private int currentButton;

    private int totalNumberOfbuttons;

    public int TotalNumberOfButtons { get { return totalNumberOfbuttons; } set { totalNumberOfbuttons = value; } }

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Debug.LogWarning("Destroyed a repeated MenuInput Manager");
            Destroy(this.gameObject);
        }

        else if (_Instance == null)
            _Instance = this;
    }


    public void Reset()
    {
        currentButton = 0;
    }

    public void moveSelection(int button)
    {
        currentButton += button;

        if (currentButton < 0) currentButton = totalNumberOfbuttons;
        else if (currentButton >= totalNumberOfbuttons) currentButton = 0;

        // Highlight selected button
        GameManager._Instance.MenuNavigation.UpdateUI(GameManager._Instance.MenuNavigation.arrayOfbuttons[currentButton]);
    }

    public void ConfirmSelection()
    {
        GameManager._Instance.MenuNavigation.SelectPressed(currentButton);
    }
}
