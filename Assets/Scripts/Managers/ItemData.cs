using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "DuoGrid/Item Data", order = 2)]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string displayName;
    public string description;
    public Sprite icon;
    public EffectType effectTypeForThisItem;
}
