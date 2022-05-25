using UnityEngine;

public class ShootingEnemy : Enemy
{
    #region Movement
    private int _currentWaypoint = 0;
    private bool _isMoving = false;
    #endregion

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        _enemyOnSight = DetectEnemy();

        if (_enemyOnSight)
        {
            // apuntamos
            // disparamos

            Aim();

            if (!_canShoot) return; // espera al firerate
            StartCoroutine(Shoot(_weaponList[0])); // el primer arma
        }
    }
}
