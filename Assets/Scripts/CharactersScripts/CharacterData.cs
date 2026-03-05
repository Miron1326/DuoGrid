using UnityEngine;
[CreateAssetMenu(fileName = "New Character Data", menuName = "DuoGrid/Character Data", order = 1)]
public class CharacterData : ScriptableObject
{
    public int health;
    public float speed;
    public AbilityData abilityForCharacter;
    public string CharacterName;
}
