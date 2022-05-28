using UnityEngine;
using System.Collections;

public abstract class Enemy : Entity
{
    #region Paremeters
    [SerializeField] protected EnemyData _enemyData;
    protected float _visionRange;
    protected int _acceleration;
    protected float _accuracy;
    #endregion

    #region BodyParts
    [Header("Body Parts")]
    [SerializeField] protected Transform _shootingPos;
    [SerializeField] protected Transform _arm;
    #endregion

    #region Target
    [SerializeField] protected LayerMask _objLayer;
    protected bool _enemyOnSight = false;
    protected Transform _targetPos;
    protected Vector3 _targetDir;
    #endregion

    #region Shooting
    protected bool _canShoot = true;
    #endregion



    protected override void Awake()
    {
        base.Awake();
        SetStats();
    }

    protected override void Start()
    {
        base.Start();
        _targetPos = GameManager.GetInstance.GetPlayerPos;
        if (_enemyData.TypeEnemy == EnemyData.EnemyType.Flying)
        {
            _bloodPool = GameManager.GetInstance.GetSparkPool;
        }
    }

    #region Parameters
    protected virtual void SetStats()
    {
        _hp = _enemyData.Hp;
        _currentHP = _hp;

        _invulnerability = _enemyData.Invulnerability;

        _acceleration = _enemyData.Acceleration;
        _speed = _enemyData.Speed;

        _accuracy = _enemyData.Accuracy;
        _visionRange = _enemyData.VisionRange;

        _weaponList = _enemyData.WeaponList;
    }
    #endregion

    #region Aiming
    /// <Summary>
    /// Da true si ve al player y false si no lo ve
    /// </Summary>
    protected bool DetectEnemy()
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

    protected override void Aim()
    {
        _targetDir = _targetPos.position - _transform.position;
        _arm.right = Vector3.Lerp(_arm.right, _targetDir.normalized, _accuracy * Time.deltaTime);
    }

    protected override IEnumerator Shoot(WeaponData weaponData)
    {
        _canShoot = false;

        yield return new WaitForSeconds(weaponData.FireRate);
        _shooting.Shoot((int)weaponData.BulletType, _shootingPos.position, _targetDir, weaponData.BulletSpeed);

        _canShoot = true;
    }
    #endregion

    #region Damage

    private void DamagePS()
    {
        GameObject blood = _bloodPool.GetPooledObject();
        if (!blood) return;

        blood.transform.position = _transform.position;
        blood.SetActive(true);
    }

    public override void TakeDamage(int value)
    {
        base.TakeDamage(value);
        DamagePS();
    }
    protected override void Death()
    {
        GameObject healthPickable = GameManager.GetInstance.GetHealthPool.GetPooledObject();
        healthPickable.transform.position = _transform.position;
        healthPickable.SetActive(true);
        gameObject.SetActive(false);
    }
    #endregion

    protected override void SetNextWaypoint()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator Melee(WeaponData weaponData)
    {
        throw new System.NotImplementedException();
    }

    #region Getter/Setter
    public EnemyData GetEnemyData
    {
        get { return _enemyData; }
    }

    public bool EnemyOnSight
    {
        get { return _enemyOnSight; }
    }
    #endregion
}
