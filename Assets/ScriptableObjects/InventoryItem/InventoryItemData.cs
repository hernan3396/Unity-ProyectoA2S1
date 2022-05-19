using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemData", menuName = "new Item", order = 0)]
public class InventoryItemData : ScriptableObject
{
    public string Id;
    public string DisplayName;
    public int MaxStack;
    public GameObject Prefab;
}