using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Entity/New Enemy")]
public class EnemyData : ScriptableObject
{
    public enum EnemyType{
        Normal,
        Flying
    }
    #region HP
    [Header("HP")]
    public int Hp;
    public float Invulnerability; // cuanto tiempo es invulnerable
    #endregion

    #region Movement
    [Header("Movement")]
    public int Acceleration;
    public int Speed; // velocidad de movimiento
    public int NewWaypointSpeed = 1;
    #endregion

    #region Shooting
    [Header("Shooting")]
    public float Accuracy; // que tan rapido es en apuntar
    public int VisionRange;
    #endregion

    #region WeaponsData
    [Header("Weapons Data")]
    public WeaponData[] WeaponList;
    #endregion
    public EnemyType TypeEnemy;
}