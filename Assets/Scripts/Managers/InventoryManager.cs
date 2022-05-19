using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour
{
    // aun no esta terminado
    [SerializeField] private InventoryItemData[] _items;

    // public void AddItem(int itemID, int value)
    // {
    //     _items[itemID].Add(value);
    // }

    // public void RemoveItem(int itemID, int value)
    // {
    //     _items[itemID].Remove(value);
    // }
}

[Serializable]
public class InventoryItem
{
    public InventoryItemData Data { get; private set; }
    public int Amount { get; private set; }

    public InventoryItem(InventoryItemData itemData)
    {
        Data = itemData;
    }

    public void Add(int value)
    {
        Amount += value;
    }

    public void Remove(int value)
    {
        Amount -= value;
    }
}