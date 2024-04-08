using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject firstButton;

    [SerializeField]
    private TextMeshProUGUI textbox;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("All Characters to display")]
    private GameObject[] listOfSpawnableCharactersWin;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("All Characters to display")]
    private GameObject[] listOfSpawnableCharactersIdle;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Location to spawn Character")]
    private Transform[] positionToSpawnCharacter;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Glow Material for each Character")]
    private Material[] glowMaterialsToAssign;

    private void Start()
    {
        Time.timeScale = 0;
        LevelLoadManager._Instance.DisableLoadingScreeen();

        EventSystem.current.SetSelectedGameObject(firstButton);
        textbox.text = "Player " + (GameManager._Instance.PlayerWinnerIndex + 1) + " Wins";

        List<int> playerWinOrder = new List<int>();     // Saved win order
        List<int> localPoints = new List<int>();        // Local save of the points

        for (int i = 0; i < PlayerStatsManager._Instance.PlayerPoints.Length; i++)
            localPoints.Add(PlayerStatsManager._Instance.PlayerPoints[i]);

        for (int j = 0; j < 4; j++)
        {
            int max = 0;    // Max saved number
            int index = 0;  // Index max number is found

            for (int i = 0; i < 4; i++) // Check the points list
            {
                if (localPoints[i] > max)
                {
                    max = localPoints[i]; // Get max number
                    index = i;
                }
            }

            localPoints[index] = -1;
            playerWinOrder.Add(index); // Most points to smallest
        }

        GameObject winChararacter;
        switch (GameManager._Instance.Players[playerWinOrder[0]].GetComponent<PlayerStats>().CharacterStat.name)
        {
            case "Cube":
                winChararacter = listOfSpawnableCharactersWin[0];
                break;
            case "Octo":
                winChararacter = listOfSpawnableCharactersWin[1];
                break;
            case "Pyr":
                winChararacter = listOfSpawnableCharactersWin[2];
                break;
            case "Twelve":
                winChararacter = listOfSpawnableCharactersWin[3];
                break;
            default:
                winChararacter = null;
                break;
        }
        GameObject characterWin = GameObject.Instantiate(winChararacter, positionToSpawnCharacter[0]);
        characterWin.transform.GetChild(0).GetComponent<Animation>().enabled = true;
        UpdateMaterialsWin(characterWin, glowMaterialsToAssign[0]);

        // Spawn players in right spots
        for (int i = 1; i < positionToSpawnCharacter.Length; i++)
        {
            GameObject characterToSpawn;
            switch (GameManager._Instance.Players[playerWinOrder[i]].GetComponent<PlayerStats>().CharacterStat.name)
            {
                case "Cube":
                    characterToSpawn = listOfSpawnableCharactersIdle[0];
                    break;
                case "Octo":
                    characterToSpawn = listOfSpawnableCharactersIdle[1];
                    break;
                case "Pyr":
                    characterToSpawn = listOfSpawnableCharactersIdle[2];
                    break;
                case "Twelve":
                    characterToSpawn = listOfSpawnableCharactersIdle[3];
                    break;
                default:
                    characterToSpawn = null;
                    break;
            }
            GameObject characterIdle = GameObject.Instantiate(characterToSpawn, positionToSpawnCharacter[i]);
            UpdateMaterialsIdle(characterIdle, glowMaterialsToAssign[playerWinOrder[i]]);
        }

        // Play audio
        StartCoroutine(WinScreenAudio());
	}
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void ButtonPressSFX()
    {
        AudioSource buttonAudioSource = AudioManager._Instance.UIAudioSource;
        AudioManager._Instance.PlaySoundFX(AudioManager._Instance.UIAudioList[1], buttonAudioSource);
    }

    public void ContinuePressed()
    {
        ButtonPressSFX();
        GameManager._Instance.EndGame();
        LevelLoadManager._Instance.StartLoadNewLevel(LevelLoadManager._Instance.LevelNamesList[0], true);
    }

    private IEnumerator WinScreenAudio()
    {
        AudioClip clipToPlay;
        switch(GameManager._Instance.Players[GameManager._Instance.PlayerWinnerIndex].GetComponent<PlayerStats>().CharacterStat.CharacterName)
        {
            case "Cube":
                clipToPlay = AudioManager._Instance.WinScreenCharacterNamesList[0];
                break;
            case "Octo":
                clipToPlay = AudioManager._Instance.WinScreenCharacterNamesList[1];
                break;
            case "Pyr":
                clipToPlay = AudioManager._Instance.WinScreenCharacterNamesList[2];
                break;
            case "Twelve":
                clipToPlay = AudioManager._Instance.WinScreenCharacterNamesList[3];
                break;
            default:
                clipToPlay = AudioManager._Instance.WinScreenCharacterNamesList[0];
                break;
        }
        AudioManager._Instance.PlaySoundFX(clipToPlay, AudioManager._Instance.MRTwentyAudioSource);

        // Wait until finished
        while (AudioManager._Instance.MRTwentyAudioSource.isPlaying)
            yield return null;

        clipToPlay = AudioManager._Instance.WinScreenWinMessageList[Random.Range(0,1)];
        AudioManager._Instance.PlaySoundFX(clipToPlay, AudioManager._Instance.MRTwentyAudioSource);
    }

    private void UpdateMaterialsWin(GameObject character, Material materialToAssign)
    {
        List<Material> listOfMaterials = new List<Material>();

        // Head
        character.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().GetMaterials(listOfMaterials);
        for (int i = 0; i < listOfMaterials.Count; i++)
            if (listOfMaterials[i].name.Contains("Light"))
                listOfMaterials[i] = materialToAssign;
        character.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().SetMaterials(listOfMaterials);

        // Face
        character.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().GetMaterials(listOfMaterials);
        for (int i = 0; i < listOfMaterials.Count; i++)
            if (listOfMaterials[i].name.Contains("Light"))
                listOfMaterials[i] = materialToAssign;
        character.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().SetMaterials(listOfMaterials);

        // Left Leg
        character.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshRenderer>().GetMaterials(listOfMaterials);
        for (int i = 0; i < listOfMaterials.Count; i++)
            if (listOfMaterials[i].name.Contains("Light"))
                listOfMaterials[i] = materialToAssign;
        character.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshRenderer>().SetMaterials(listOfMaterials);

        // Right Leg
        character.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<MeshRenderer>().GetMaterials(listOfMaterials);
        for (int i = 0; i < listOfMaterials.Count; i++)
            if (listOfMaterials[i].name.Contains("Light"))
                listOfMaterials[i] = materialToAssign;
        character.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<MeshRenderer>().SetMaterials(listOfMaterials);
    }

    private void UpdateMaterialsIdle(GameObject character, Material materialToAssign)
    {
        List<Material> listOfMaterials = new List<Material>();

        // Head
        character.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().GetMaterials(listOfMaterials);
        for (int i = 0; i < listOfMaterials.Count; i++)
            if (listOfMaterials[i].name.Contains("Light"))
                listOfMaterials[i] = materialToAssign;
        character.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().SetMaterials(listOfMaterials);

        // Face
        character.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().GetMaterials(listOfMaterials);
        for (int i = 0; i < listOfMaterials.Count; i++)
            if (listOfMaterials[i].name.Contains("Light"))
                listOfMaterials[i] = materialToAssign;
        character.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().SetMaterials(listOfMaterials);

        // Left Leg
        character.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().GetMaterials(listOfMaterials);
        for (int i = 0; i < listOfMaterials.Count; i++)
            if (listOfMaterials[i].name.Contains("Light"))
                listOfMaterials[i] = materialToAssign;
        character.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().SetMaterials(listOfMaterials);

        // Right Leg
        character.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshRenderer>().GetMaterials(listOfMaterials);
        for (int i = 0; i < listOfMaterials.Count; i++)
            if (listOfMaterials[i].name.Contains("Light"))
                listOfMaterials[i] = materialToAssign;
        character.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshRenderer>().SetMaterials(listOfMaterials);
    }
}
