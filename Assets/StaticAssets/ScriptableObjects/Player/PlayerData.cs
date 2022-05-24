using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "Entity/Player Data", order = 0)]
public class PlayerData : ScriptableObject
{
    #region HP
    [Header("HP")]
    public int Hp;
    public float Invulnerability;
    #endregion

    #region Jumping
    [Header("Jumping")]
    public int GravityScale; // no esta
    public int JumpForce; // no esta
    public int JumpTime; // no esta
    #endregion

    #region Movement
    [Header("Movement")]
    public int Speed;
    #endregion

    #region WeaponsData
    [Header("Weapons Data")]
    [SerializeField] public WeaponData[] WeaponList;
    #endregion
}