using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    #region Components
    private PoolManager _bloodPool;
    #endregion

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
        _bloodPool = GameManager.GetInstance.GetBloodPool;
        _shooting = GameManager.GetInstance.GetShooting;
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        _enemyOnSight = DetectEnemy();
        if (!_enemyOnSight) return; // si ve a un enemigo empieza a ejecutarse el resto del codigo

        Aim();
        if (!_canShoot) return; // espera al firerate
        StartCoroutine("Shoot");
    }

    private void SetStats()
    {
        _hp = _enemyData.Hp;
        _currentHP = _hp;

        _fireRate = _enemyData.FireRate;
        _accuracy = _enemyData.Accuracy;
        _bulletSpeed = _enemyData.BulletSpeed;
        _movSpeed = _enemyData.MovSpeed;
        _invulnerability = _enemyData.Invulnerability;
        _visionRange = _enemyData.VisionRange;
    }

    #region Aiming
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

    private IEnumerator Shoot()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_fireRate);
        _shooting.Shoot((int)InventoryManager.ItemID.Bullet, _shootingPos.position, _targetDir, _bulletSpeed);

        _canShoot = true;
    }
    #endregion

    #region Damage
    public void TakeDamage(int value)
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

    private IEnumerator InmuneReset()
    {
        yield return new WaitForSeconds(_invulnerability);
        _isInmune = false;
    }

    private void Death()
    {
        GameObject healthPickable = GameManager.GetInstance.GetHealthPool.GetPooledObject();
        healthPickable.transform.position = transform.position;
        healthPickable.SetActive(true);
        gameObject.SetActive(false);
    }
    #endregion

    public bool EnemyOnSight
    {
        get { return _enemyOnSight; }
    }
}
