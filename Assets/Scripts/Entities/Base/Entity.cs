using UnityEngine;
using System.Collections;

public abstract class Entity : MonoBehaviour
{
    #region Components
    protected PoolManager _bloodPool;
    protected Transform _transform;
    protected Shooting _shooting;
    #endregion
    #region Parameters
    [Header("Parameters")]
    protected int _hp;
    protected int _speed;
    protected float _invulnerability;
    protected bool _isInmune;
    protected int _currentHP;
    protected WeaponData[] _weaponList;
    #endregion

    protected virtual void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    protected virtual void Start()
    {
        _bloodPool = GameManager.GetInstance.GetBloodPool;
        _shooting = GameManager.GetInstance.GetShooting;

    }

    protected abstract void Move();
    protected abstract void Aim();
    protected abstract IEnumerator Shoot(WeaponData weaponData);
    protected abstract IEnumerator Melee();
    protected abstract void Death();

    public virtual void TakeDamage(int value)
    {
        if (_isInmune) return;
        _isInmune = true;
        StartCoroutine("InmuneReset");

        _currentHP -= value;

        GameObject blood = _bloodPool.GetPooledObject();
        if (!blood) return;

        blood.transform.position = _transform.position;
        blood.SetActive(true);

        if (_currentHP <= 0)
        {
            Death();
        }
    }

    protected IEnumerator InmuneReset()
    {
        yield return new WaitForSeconds(_invulnerability);
        _isInmune = false;
    }
}
