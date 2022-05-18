using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "new Weapon", order = 0)]
public class WeaponData : ScriptableObject
{
    public string Name;
    public int BulletSpeed;
    public float FireRate;
    public Shooting.BulletType BulletType; // es esto una forma correcta de hacerlo??
}