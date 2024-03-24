using NaughtyAttributes;
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


    private float playerEmissionIntensity = 2f;

    private void Start()
    {
        for (int i = 0; i < positionToSpawnCharacter.Length; i++)
        {
            GameObject character = GameObject.Instantiate(listOfSpawnableCharacters[i], positionToSpawnCharacter[i]);
            UpdateMaterials(character, glowMaterialsToAssign[i]);
        }
    }

    public void UpdateCharacterDisplay(int playerIndex, int selectedCharacter)
    {
        Destroy(positionToSpawnCharacter[playerIndex].GetChild(0).gameObject);
        GameObject character = GameObject.Instantiate(listOfSpawnableCharacters[selectedCharacter], positionToSpawnCharacter[playerIndex]);
        UpdateMaterials(character, glowMaterialsToAssign[playerIndex]);
    }

    private void UpdateMaterials(GameObject character, Material materialToAssign)
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

    public void ResetMaterialEmissionColor(int playerIndex, Texture color, Color uiColor)
    {
        // Finding how close to white the player's colour is
        // Note that it is only divided by 2 * 255 and not 3
        // This is only so that the amount of brightness taken from the emission is greater than 1 (has more effect).
        float colourBrightness = uiColor.r + uiColor.g + uiColor.b;
        colourBrightness /= (255 * 3);

        ColorMutator playerEmission = new(uiColor);
        playerEmission.exposureValue = playerEmissionIntensity - colourBrightness;
        
        // Setting the player lights material values
        glowMaterialsToAssign[playerIndex].SetTexture("_EmissionMap", color);
        glowMaterialsToAssign[playerIndex].SetColor("_BaseColor", uiColor);
        glowMaterialsToAssign[playerIndex].EnableKeyword("_EMISSION");
        glowMaterialsToAssign[playerIndex].SetColor("_EmissionColor", playerEmission.exposureAdjustedColor); // To convert from the regular colour to HDR (with intensity), multiply the intensity value into the colour. We subtract colourBrightness from intensity so the lighter colours aren't as blown out.
    }
}
