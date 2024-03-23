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

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Glow Material for each Character")]
    private Material[] glowMaterialsToAssign;

    private void Start()
    {
        for (int i = 0; i < positionToSpawnCharacter.Length; i++)
            GameObject.Instantiate(listOfSpawnableCharacters[i], positionToSpawnCharacter[i]);
    }

    public void UpdateCharacterDisplay(int playerIndex, int selectedCharacter)
    {
        Destroy(positionToSpawnCharacter[playerIndex].GetChild(0).gameObject);
        GameObject.Instantiate(listOfSpawnableCharacters[selectedCharacter], positionToSpawnCharacter[playerIndex]);
    }
}
