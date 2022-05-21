using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Entity/New Enemy")]
public class EnemyData : ScriptableObject
{
    public int Hp;
    public int Damage;
    public float FireRate; // cadencia de disparo
    public float Accuracy; // que tan rapido es en apuntar
    public int BulletSpeed; // velocidad de la bala que dispara
    public int MovSpeed; // velocidad de movimiento
    public float Invulnerability; // cuanto tiempo es invulnerable
    public int VisionRange;
}