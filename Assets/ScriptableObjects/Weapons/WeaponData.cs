using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "new Weapon", order = 0)]
public class WeaponData : ScriptableObject
{
    // pasar el bullettype para aca
    // y ajustar el shooting
    public string Name;
    public int BulletSpeed;
    public float FireRate;
    public Shooting.BulletType BulletType;
}