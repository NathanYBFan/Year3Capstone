using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugMenu : MonoBehaviour
{

	// Singleton Initialization
	public static DebugMenu _Instance;

	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private TMP_InputField commandEntry;
	[SerializeField]
	[Foldout("Dependencies"), Tooltip("")]
	private GameObject debugMenu;
	private string command = "";
    private bool isVisible = false;

	private void Awake()
	{
		if (_Instance != null && _Instance != this)
		{
			Debug.LogWarning("Destroyed a repeated DebugMenu");
			Destroy(this.gameObject);
		}

		else if (_Instance == null)
			_Instance = this;
	}
	public void OnDebugPressed()
    {
		isVisible = !isVisible;
		debugMenu.SetActive(isVisible);
    }

	public void OnCommandEntered()
	{
		command = commandEntry.text;
		ProcessCommand(command);
		commandEntry.text = "";
	}

	public void ProcessCommand(string command)
	{
		string[] commandParts = command.Split(' ');
		string mainCommand = commandParts[0].ToLower();
		switch (mainCommand)
		{
			case "give":
				if (commandParts.Length == 3)
				{
					int index;
					if (int.TryParse(commandParts[1], out index))
					{
						string modifierName = commandParts[2];
						GameManager._Instance.CommandGive(index, modifierName);
					}
					else Debug.LogWarning("Invalid Player Index in give command!");
				}
				else if (commandParts.Length < 3) Debug.LogWarning("Not enough parameters for the give command!");
				else Debug.LogWarning("Too many parameters for the give command!");
				break;
			default:
				Debug.LogWarning("Invalid command!");
				break;
		}
	}
}
