using UnityEngine;
using System.Collections;

public abstract class NewEnemy : Entity
{
    #region Components

    #endregion

    #region Paremeters
    [SerializeField] protected EnemyData _enemyData;
    protected float _accuracy;
    protected float _visionRange;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        SetStats();
    }

    protected virtual void SetStats()
    {
        _hp = _enemyData.Hp;
        _currentHP = _hp;

        _invulnerability = _enemyData.Invulnerability;

        _speed = _enemyData.Speed;

        _accuracy = _enemyData.Accuracy;
        _visionRange = _enemyData.VisionRange;

        _weaponList = _enemyData.WeaponList;
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
}
