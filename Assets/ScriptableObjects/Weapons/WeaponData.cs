using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/New Weapon", order = 0)]
public class WeaponData : ScriptableObject
{
    // pasar el bullettype para aca
    // y ajustar el shooting
    public string Name;
    public int BulletSpeed;
    public float FireRate;
    public InventoryManager.ItemID BulletType;
    // pase el enum a InventoryManager para tenerlos en un lado solo
    // y me parecio el correcto ahi
}