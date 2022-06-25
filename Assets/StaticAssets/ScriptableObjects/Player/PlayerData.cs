using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "Entity/Player Data", order = 0)]
public class PlayerData : ScriptableObject
{
    #region HP
    [Header("HP")]
    public int Hp;
    public float DeathDuration;
    public float Invulnerability;
    #endregion

    #region Jumping
    [Header("Jumping")]
    public int GravityScale;
    public int JumpForce;
    public int JumpTime;
    public int FallingMaxSpeed;
    #endregion

    #region Movement
    [Header("Movement")]
    public int Speed;
    public int RocketJumpingSpeed;
    #endregion

    #region CameraShake
    [Header("Camera Shake")]
    public float DamageShake;
    public float ShakeTime;
    #endregion

    #region WeaponsData
    [Header("Weapons Data")]
    public WeaponData[] WeaponList;
    public float Accuracy;
    #endregion
}