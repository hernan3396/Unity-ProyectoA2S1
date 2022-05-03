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
    private int _visionRange;
    #endregion

    #region BodyParts
    [Header("Body Parts")]
    [SerializeField] private Transform _shootingPos;
    [SerializeField] private Transform _arm;
    private Transform _transform;
    private Rigidbody _rb;
    #endregion

    #region Target
    [Header("Target")]
    [SerializeField] private LayerMask _objLayer;
    private bool _enemyOnSight = false;
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
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _enemyOnSight = DetectEnemy();
        if (!_enemyOnSight) return; // si ve a un enemigo empieza a ejecutarse el resto del codigo

        Aim();
        if (!_canShoot) return; // espera al firerate
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
        _visionRange = _enemyData.VISIONRANGE;
    }

    /// <Summary>
    /// Da true si ve al player y false si no lo ve
    /// </Summary>
    private bool DetectEnemy()
    {
        // Debug.DrawRay(_transform.position, _targetPos.position - _transform.position, Color.blue);

        // si el enemigo esta a la vista deja de patrullar y le dispara
        if (Physics.Raycast(_transform.position, _targetPos.position - _transform.position, out RaycastHit hit, _visionRange, _objLayer))
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                return true;
            }

        return false;
    }

    /// <Summary>
    /// Apunta el brazo en la direccion del player
    /// </Summary>
    private void Aim()
    {
        _targetDir = _targetPos.position - _transform.position;
        _arm.right = Vector3.Lerp(_arm.right, _targetDir.normalized, _accuracy * Time.deltaTime);
        // en un update lo mejor es no usar dotween
        // _arm.DORotate(_targetDir.normalized, _accuracy);
        // DOTween.To(() => _arm.right, x => _arm.right = x, _targetDir, _accuracy); // mueve el brazo
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

    public bool EnemyOnSight
    {
        get { return _enemyOnSight; }
    }

    public Rigidbody GetRB
    {
        get { return _rb; }
    }
}
