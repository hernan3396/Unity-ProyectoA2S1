using UnityEngine;
using TMPro;

public class BossTurret : Enemy
{
    [SerializeField] private TMP_Text _textState;

    protected override void Awake()
    {
        base.Awake();
        _currentState = States.Wandering;
    }

    private void Update()
    {
        _textState.text = _currentState.ToString();

        switch (_currentState)
        {
            case States.Shooting:
                Shoot();
                break;
            case States.Wandering:
                Wandering();
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
    }

    private void NewState(States newState)
    {
        _currentState = newState;
    }
}
