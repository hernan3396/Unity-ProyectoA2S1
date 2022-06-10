using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour
{
    // la vida deberia ser un item?
    public enum ItemID
    {
        PlayerBullet,
        Rocket,
        EnemyBullet,
        None
    }

    #region Components
    private UIController _uiController;
    private SavesManager _savesManager;
    #endregion

    [SerializeField] private InventoryItemData[] _itemData; // los prefabs con info
    [SerializeField] private InventoryItem[] _items; // la lista de items

    private void Start()
    {
        _uiController = GameManager.GetInstance.GetUIController;
        _savesManager = GameManager.GetInstance.GetSavesManager;

        PopulateInventory();
    }

    // creates initial list for items
    private void PopulateInventory()
    {
        // esta medio feo pero para esto
        // los playerprefs deberian tener algo asi
        // como una ID para poder identificarlos mas facil
        // pero como son solo 2 items que hay que recordar
        // quedara asi
        _items = new InventoryItem[_itemData.Length];

        for (int i = 0; i < _itemData.Length; i++)
        {
            _items[i] = new InventoryItem();
            _items[i].SetData(_itemData[i]);
        }

        int index;

        index = (int)ItemID.PlayerBullet;
        _items[index].SetAmount(_savesManager.GetBulletAmount);

        _uiController.UpdateItemText(index, _items[index].GetCurrentAmonut);

        index = (int)ItemID.Rocket;
        _items[index].SetAmount(_savesManager.GetRocketAmount);

        _uiController.UpdateItemText(index, _items[index].GetCurrentAmonut);
    }

    public void AddAmount(int itemID, int value)
    {
        _items[itemID].AddAmount(value);
        // seguro se puede hacer distinto esto
        // pero aun ni tenemos ui hecha mas que 
        // place holders asi que aguanta
        _uiController.UpdateItemText(itemID, _items[itemID].GetCurrentAmonut);
    }

    public void RemoveAmount(int itemID, int value)
    {
        _items[itemID].RemoveAmount(value);
        _uiController.UpdateItemText(itemID, _items[itemID].GetCurrentAmonut);
    }

    public int GetAmount(int itemID) => _items[itemID].GetCurrentAmonut;

    // rellena completamente las balas
    public void Restock()
    {
        // es un metodo para debugging
        // asi que no importa que haya quedado
        // super raro escrito
        _items[(int)ItemID.PlayerBullet].SetAmount(_itemData[(int)ItemID.PlayerBullet].MaxStack);
        _uiController.UpdateItemText((int)ItemID.PlayerBullet, _items[(int)ItemID.PlayerBullet].GetCurrentAmonut);
        _items[(int)ItemID.Rocket].SetAmount(_itemData[(int)ItemID.Rocket].MaxStack);
        _uiController.UpdateItemText((int)ItemID.Rocket, _items[(int)ItemID.Rocket].GetCurrentAmonut);
    }
}

[Serializable]
public class InventoryItem
{
    // si necesitas debuggear pasar estas privadas
    // a publicas, para eso el [Serializable]
    private InventoryItemData Data; // data del item
    private int Amount; // cantidad de items que tiene

    public void SetData(InventoryItemData itemData) => Data = itemData;
    public void AddAmount(int value)
    {
        Amount += value;

        if (Amount > Data.MaxStack)
            Amount = Data.MaxStack;
    }
    public void RemoveAmount(int value)
    {
        Amount -= value;

        if (Amount < 0)
            Amount = 0;
    }

    public void SetAmount(int value)
    {
        Amount = value;

        if (Amount > Data.MaxStack)
            Amount = Data.MaxStack;
    }

    #region Getter/Setter
    public int GetCurrentAmonut
    {
        get { return Amount; }
    }

    public int GetMaxStack
    {
        get { return Data.MaxStack; }
    }
    #endregion
}