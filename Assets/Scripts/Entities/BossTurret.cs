using UnityEngine;
using TMPro;

public class BossTurret : Enemy
{
    [SerializeField] private TMP_Text _textState;
    private BossMain _bossMain;
    private bool _isPaused = false;

    protected override void Awake()
    {
        base.Awake();
        _currentState = States.Wandering;

        _bossMain = GetComponentInParent<BossMain>();
    }

    private void Update()
    {
        if (!_bossMain.Active) return;
        if (_isPaused) return;

        _textState.text = _currentState.ToString();

        switch (_currentState)
        {
            case States.Shooting:
                Shoot();
                break;
            case States.Wandering:
                Wandering();
                break;
            case States.Smashing:
                Smashing();
                break;
        }
    }

    private void Wandering()
    {
        _enemyOnSight = DetectEnemy();

        if (_enemyOnSight)
            NewState(States.Shooting);
    }

    private void Shoot()
    {
        Aim();
        _enemyOnSight = DetectEnemy();

        if (_canShoot)
            StartCoroutine(Shoot(_weaponList[0]));

        if (!_enemyOnSight)
            NewState(States.Wandering);

        if (!_bossMain.CanAttack)
            NewState(States.Smashing);
    }

    private void Smashing()
    {
        if (_bossMain.CanAttack)
            NewState(States.Wandering);
    }

    private void NewState(States newState)
    {
        _currentState = newState;
    }
}
