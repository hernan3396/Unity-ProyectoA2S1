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

    #region Recoil
    [Header("Recoil")]
    public int RecoilForce;
    public float RecoilTime;
    #endregion

    #region CameraShake
    [Header("Camera Shake")]
    public float ShootShake;
    public float ShakeTime;
    #endregion

    // no me gusta como esta hecho pero
    // tampoco se me ocurrio como hacerlo
    // distinto ya que el daño de las armas
    // depende de que bala disparen y no del
    // daño en si de ellas mismas, a lo mejor
    // se podria hacer un sistema como el del melee
    // donde el proyectil que dispara tenga un valor
    // dependiendo del arma que la dispare
    [Header("Melee Damage")]
    public int MeleeDamage;
}