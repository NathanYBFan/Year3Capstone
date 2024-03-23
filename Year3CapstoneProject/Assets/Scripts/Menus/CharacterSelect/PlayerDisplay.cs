using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDisplay : MonoBehaviour
{
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("All Characters to display")]
    private GameObject[] listOfSpawnableCharacters;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Location to spawn Character")]
    private Transform[] positionToSpawnCharacter;

    private void Start()
    {
        for (int i = 0; i < positionToSpawnCharacter.Length; i++)
            GameObject.Instantiate(listOfSpawnableCharacters[i], positionToSpawnCharacter[i]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            Destroy(positionToSpawnCharacter[1].GetChild(0).gameObject);
    }

    public void UpdateCharacterDisplay(int playerIndex, int selectedCharacter)
    {
        Destroy(positionToSpawnCharacter[playerIndex].GetChild(0).gameObject);
        GameObject.Instantiate(listOfSpawnableCharacters[selectedCharacter], positionToSpawnCharacter[playerIndex]);
    }
}
