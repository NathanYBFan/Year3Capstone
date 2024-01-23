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

	/// <summary>
	/// Show or hide debug menu.
	/// </summary>
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

	/// <summary>
	/// This method processes a given command and splits its arguments for the GameManager to understand.
	/// </summary>
	/// <param name="command">The given command in the command entry.</param>
	public void ProcessCommand(string command)
	{
		string[] commandParts = command.Split(' ');
		string mainCommand = commandParts[0].ToLower();

		//The command
		switch (mainCommand)
		{
			case "give":
				//Does it have the right number of parameters?
				if (commandParts.Length == 3)
				{
					int index;
					//Is this a valid number?
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