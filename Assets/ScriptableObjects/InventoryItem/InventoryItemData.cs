using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Weapons/New Item", order = 0)]
public class InventoryItemData : ScriptableObject
{
    public string Id;
    public string DisplayName;
    public int MaxStack;
}