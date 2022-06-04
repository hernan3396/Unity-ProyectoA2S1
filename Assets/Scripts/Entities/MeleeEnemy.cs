using UnityEngine.AI;
using UnityEngine;
using System.Collections;

public class MeleeEnemy : Enemy
{
    #region AI
    private NavMeshAgent _agent;
    #endregion

    #region Melee
    [Header("Melee")]
    [SerializeField] private GameObject _meleeWeapon;
    private bool _enemyOnRange;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
        _meleeWeapon.SetActive(false); // por si nos olvidamos de dejarla apagada
    }

    private void Update()
    {
        _enemyOnSight = DetectEnemy();
        ManageState();

        if (_enemyOnSight)
        {
            _agent.SetDestination(_targetPos.position);
            Aim();
        }

        if (_enemyOnRange)
        {
            // atacar de melee
            if (!_canShoot) return; // espera al firerate
            StartCoroutine(Melee(_weaponList[0])); // el primer arma
        }
    }

    #region MeleeAttack
    protected override IEnumerator Melee(WeaponData weaponData)
    {
        _canShoot = false;

        yield return new WaitForSeconds(weaponData.FireRate);
        _meleeWeapon.SetActive(true); // prende la hitbox

        yield return new WaitForSeconds(weaponData.FireRate / 2); // lo dividi entre 2 porque si
        // ademas hace que no este activa tanto tiempo esta hitbox
        // y no tenemos que andar creando otro valor en EnemyData
        _meleeWeapon.SetActive(false);

        _canShoot = true;
    }
    #endregion

    #region MeleeRange
    public void EnemyOnRange()
    {
        _enemyOnRange = true;
    }

    public void EnemyOutRange()
    {
        _enemyOnRange = false;
    }
    #endregion
}
