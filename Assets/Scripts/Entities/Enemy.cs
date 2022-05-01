using UnityEngine;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class Enemy : MonoBehaviour
{
    #region Parameters
    [Header("Parameters")]
    [SerializeField] private EnemyData _enemyData;
    // parece que estan medios desordenados pero es el
    // orden que estan en el scriptable object
    // asi mantenemos coherencia entre ambos
    private int _hp; // vida maxima
    private int _currentHP; // la vida actual
    private float _fireRate;
    private float _accuracy;
    private int _bulletSpeed;
    private int _movSpeed;
    private float _invulnerability;
    #endregion

    #region BodyParts
    [Header("Body Parts")]
    [SerializeField] private Transform _shootingPos;
    [SerializeField] private Transform _arm;
    private Transform _transform;
    #endregion

    #region Target
    private Transform _targetPos;
    private Vector3 _targetDir;
    #endregion

    #region Shooting
    private Shooting _shooting;
    private bool _canShoot = true;
    #endregion

    private bool _isInmune;

    private void Awake()
    {
        SetStats();
    }

    private void Start()
    {
        _targetPos = GameManager.GetInstance.GetPlayerPos;
        _shooting = GameManager.GetInstance.GetShooting;
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        Aim();

        if (!_canShoot) return;
        StartCoroutine("Shooting");
    }

    private void SetStats()
    {
        _hp = _enemyData.HP;
        _currentHP = _hp;

        _fireRate = _enemyData.FIRERATE;
        _accuracy = _enemyData.ACCURACY;
        _bulletSpeed = _enemyData.BULLETSPEED;
        _movSpeed = _enemyData.MOVSPEED;
        _invulnerability = _enemyData.INVULNERABILITY;
    }

    private void Aim()
    {
        _targetDir = _targetPos.position - _transform.position;
        DOTween.To(() => _arm.right, x => _arm.right = x, _targetDir, _accuracy);
    }

    private IEnumerator Shooting()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_fireRate);
        _shooting.Shoot(_shootingPos.position, _targetDir, _bulletSpeed);

        _canShoot = true;
    }

    public void TakeDamage(int value)
    {
        if (_isInmune) return;
        _isInmune = true;
        StartCoroutine("InmuneReset");

        _currentHP -= value;

        if (_currentHP <= 0)
        {
            Death();
        }
    }

    private IEnumerator InmuneReset()
    {
        yield return new WaitForSeconds(_invulnerability);
        _isInmune = false;
    }

    private void Death()
    {
        Debug.Log("Murio un enemigo");
        gameObject.SetActive(false);
    }
}
