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
    public int GravityScale;
    public float ParticlePossOff;
    public int JumpForce;
    public int JumpTime;
    #endregion

    #region Movement
    [Header("Movement")]
    public int Speed;
    #endregion

    #region Rocket Jumping
    [Header("Rocket Jumping")]
    public float RocketJumpingTimer;
    #endregion

    #region WeaponsData
    [Header("Weapons Data")]
    [SerializeField] private WeaponData[] _weaponList;
    #endregion
}