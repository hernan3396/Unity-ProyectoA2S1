public class ShootingEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (_onPause) return;

        _enemyOnSight = DetectEnemy();
        ManageState();

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
