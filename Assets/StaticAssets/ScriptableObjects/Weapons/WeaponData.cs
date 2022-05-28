using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/New Weapon", order = 0)]
public class WeaponData : ScriptableObject
{
    // serian las player weapons
    public enum Weapons
    {
        TwinPistols,
        RocketLauncher,
        // EnemyPistol, no es necesario esto porque el enemigo siempre tiene 1 arma
        // entnces llamas al primer indice del arreglo
        Bat
    }

    public string Name;
    public int BulletSpeed;
    public float FireRate;
    public InventoryManager.ItemID BulletType;
    // pase el enum a InventoryManager para tenerlos en un lado solo
    // y me parecio el correcto ahi

    #region CameraShake
    [Header("Camera Shake")]
    public float ShootShake;
    public float ShakeTime;
    #endregion
}