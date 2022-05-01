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
    private int _hp;
    private int _damage;
    private float _fireRate;
    private float _accuracy;
    private int _bulletSpeed;
    private int _movSpeed;
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

    private PoolManager _bulletPool;

    private void Awake()
    {
        SetStats();
    }

    private void Start()
    {
        _bulletPool = GameManager.GetInstance.GetEnemyBulletPool;
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
        _damage = _enemyData.DAMAGE;
        _fireRate = _enemyData.FIRERATE;
        _accuracy = _enemyData.ACCURACY;
        _bulletSpeed = _enemyData.BULLETSPEED;
        _movSpeed = _enemyData.MOVSPEED;
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

        _canShoot = true;
        _shooting.Shoot(_shootingPos.position, _targetDir, _bulletSpeed);
    }
}
