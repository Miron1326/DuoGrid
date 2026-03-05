using UnityEngine;

[CreateAssetMenu(fileName = "New Ability Data", menuName = "DuoGrid/Ability Data", order = 1)]
public class AbilityData : ScriptableObject
{
    public int damage;
    public int cooldown;
    public string nameAbility;
    public int range;

}
