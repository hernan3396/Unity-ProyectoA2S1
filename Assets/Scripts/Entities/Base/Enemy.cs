using UnityEngine;
using System.Collections;

public abstract class Enemy : Entity
{
    protected enum States
    {
        Shooting,
        Wandering
    }

    #region Paremeters
    [SerializeField] protected EnemyData _enemyData;
    protected States _currentState;
    protected float _shootAccuracy;
    protected float _visionRange;
    protected int _acceleration;
    protected float _accuracy;
    protected bool _melee = false;
    #endregion

    #region BodyParts
    [Header("Body Parts")]
    [SerializeField] protected Animator _modelAnimator;
    [SerializeField] protected Transform _shootingPos;
    [SerializeField] protected Transform _model;
    [SerializeField] protected Transform _arm;
    #endregion

    #region Target
    [SerializeField] private Vector3 _targetOffset = new Vector3(0, 2, 0);
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
        _shootAccuracy = _enemyData.ShootAccuracy;

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
        _targetDir = _targetPos.position - _arm.position;
        // Debug.DrawRay(_transform.position, _targetDir, Color.blue);
        _arm.right = Vector3.Lerp(_arm.right, _targetDir.normalized, _accuracy * Time.deltaTime);

        if (_arm.right.x > 0)
            _model.forward = new Vector3(1, 0, 0);
        else
            _model.forward = new Vector3(-1, 0, 0);
    }

    protected override IEnumerator Shoot(WeaponData weaponData)
    {
        _canShoot = false;

        yield return new WaitForSeconds(weaponData.FireRate);
        Vector3 shootOff = GenerateRandomVector(_shootAccuracy);
        _shooting.Shoot((int)weaponData.BulletType, _shootingPos.position, _targetDir + shootOff, weaponData.BulletSpeed);

        _canShoot = true;
    }

    protected Vector3 GenerateRandomVector(float range)
    {
        float xValue = Random.Range(-range, range);
        float yValue = Random.Range(-range, range);
        return new Vector3(xValue, yValue, 0);
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

    public bool SetMeleeDamage
    {
        get { return _melee; }
        set { _melee = value; }
    }

    public override void TakeDamage(int value)
    {
        base.TakeDamage(value);
        DamagePS();
    }
    protected override void Death()
    {

        if (_melee)
        {
            GameObject ammoPickable = GameManager.GetInstance.GetAmmoPool.GetPooledObject();
            ammoPickable.transform.position = _transform.position;
            ammoPickable.SetActive(true);
        }
        else
        {
            GameObject healthPickable = GameManager.GetInstance.GetHealthPool.GetPooledObject();
            healthPickable.transform.position = _transform.position;
            healthPickable.SetActive(true);
        }
        if (gameObject.GetComponentInChildren<Ragdoll>())
        {
            gameObject.GetComponentInChildren<Ragdoll>().DeathRagdoll();
        }
        else
        {
            gameObject.SetActive(false);
        }

    }
    #endregion

    #region States
    protected void ManageState()
    {
        if (_enemyOnSight)
        {
            ChangeState(States.Shooting);
            return;
        }

        ChangeState(States.Wandering);
    }

    protected void ChangeState(States newState)
    {
        _currentState = newState;
        _modelAnimator.Play(newState.ToString());
    }
    #endregion

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
