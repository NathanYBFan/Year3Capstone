using UnityEngine;

[CreateAssetMenu(menuName = "Character Stat", order = 2)]
public class CharacterStatsSO : ScriptableObject
{
    [Header("Stats")]
    public string CharacterName;
    public GameObject playerModelHead;
    public GameObject playerModelBody;
    public Sprite characterSprite;
    public Sprite characterHurtSprite;
    public Sprite characterBGSprite;
    public int MaxHealth;
    public float DefaultMoveSpeed;
    public float DefaultFireRate;
    public float MaxEnergy;
    public float EnergyRegenRate;
}
