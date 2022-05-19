using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/New Weapon", order = 0)]
public class WeaponData : ScriptableObject
{
    public enum BulletType
    {
        Bullet,
        Rocket,
        None
    }

    // pasar el bullettype para aca
    // y ajustar el shooting
    public string Name;
    public int BulletSpeed;
    public float FireRate;
    public BulletType AmmoType;
}