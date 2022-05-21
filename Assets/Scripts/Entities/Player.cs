using UnityEngine;
using System.Collections;

public class Player : Entity
{
    #region Components
    private UIController _uiController;
    private Rigidbody _rb;
    private Inputs _input;
    private Camera _cam;
    #endregion

    #region Parameters
    [SerializeField] private PlayerData _playerData;
    private int _gravityScale;
    private int _jumpForce;
    private int _jumpTime;
    #endregion

    #region BodyParts
    private Transform _particlePossOff;
    #endregion

    #region RocketJumping
    private float _rocketJumpingTimer;
    #endregion


    protected override void Awake()
    {
        base.Awake();
        SetStats();
    }

    private void SetStats()
    {
        _hp = _playerData.Hp;
        _currentHP = _hp;
        _invulnerability = _playerData.Invulnerability;

        _gravityScale = _playerData.GravityScale;
        _jumpForce = _playerData.JumpForce;
        _jumpTime = _playerData.JumpTime;

        _speed = _playerData.Speed;

        _rocketJumpingTimer = _playerData.RocketJumpingTimer;

        _weaponList = _playerData.WeaponList;
    }

    protected override void Move()
    {
        throw new System.NotImplementedException();
    }

    protected override void Aim()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator Shoot(WeaponData weaponData)
    {
        yield return null;
    }

    protected override IEnumerator Melee()
    {
        throw new System.NotImplementedException();
    }

    protected override void Death()
    {
        throw new System.NotImplementedException();
    }

    public void RocketJumping(bool value)
    {
        Debug.Log("Test");
    }

    public void AddHealth(int value)
    {
        Debug.Log("Test");
    }

    public Rigidbody GetRB
    {
        get { return _rb; }
    }
}
