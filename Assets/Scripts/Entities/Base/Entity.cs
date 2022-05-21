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
    [SerializeField] protected int _maxHP;
    [SerializeField] protected int _movSpeed;
    [SerializeField] protected float _fireRate;
    [SerializeField] protected int _bulletSpeed;
    [SerializeField] protected float _invulnerability;
    [SerializeField] protected float _meleeSpeed;
    protected bool _isInmune;
    protected int _currentHP;
    #endregion

    protected void Awake()
    {
        _currentHP = _maxHP;
        _transform = GetComponent<Transform>();
    }

    protected void Start()
    {
        _shooting = GetComponent<Shooting>();
    }

    protected abstract void Move();
    protected abstract void Aim();
    protected abstract IEnumerator Shoot(WeaponData weaponData);
    protected abstract IEnumerator Melee();
    protected abstract void Death();
    protected void TakeDamage(int value)
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
