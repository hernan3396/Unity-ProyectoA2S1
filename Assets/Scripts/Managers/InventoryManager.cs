using UnityEngine;
using System;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    // aun no esta terminado
    public List<InventoryItem> Inventory { get; private set; } = new List<InventoryItem>();

    public void Add()
    {
    }
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