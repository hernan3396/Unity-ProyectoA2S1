using UnityEngine;
using TMPro;
using DG.Tweening;

public class BossMain : Enemy
{
    [Header("Settings")]
    [SerializeField] private Transform[] _bossPoints; // para la intro del boss 
    [SerializeField] private TMP_Text _textState;
    private UIController _uiController;

    #region Hands
    [Header("Hands")]
    [SerializeField] private GameObject[] _hands;
    [SerializeField] private Transform[] _handsEndPos;
    [SerializeField] private Transform[] _handsStartPos;
    private Vector3[] _handSpawnPos; // para cuando reinicias el boss
    private int _handIndex = 0;
    #endregion

    #region Flags
    private bool _canAttack = true;
    private bool _returning = false;
    private bool _active = false;
    private float _atkTimer;
    #endregion

    #region EnemySpawner
    [Header("Enemy Spawner")]
    [SerializeField] private EnemySpawner _enemySpawner;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        _currentState = States.Wandering;
    }

    protected override void Start()
    {
        base.Start();
        _uiController = GameManager.GetInstance.GetUIController;

        int index = 0;
        _handSpawnPos = new Vector3[_hands.Length];
        foreach (GameObject hand in _hands)
        {
            _handSpawnPos[index] = hand.transform.position;
            index += 1;
        }
    }

    private void Update()
    {
        if (!_active) return;
        if (_onPause) return;
        if (_isGameOver) return;
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
                Smash();
                break;
            case States.Returning:
                ReturnHand();
                break;
        }
    }

    private void Wandering()
    {
        _enemyOnSight = DetectEnemy();
        _atkTimer = 0;

        if (_enemyOnSight)
            NewState(States.Shooting);
    }

    private void Shoot()
    {
        _enemyOnSight = DetectEnemy();

        _atkTimer += Time.deltaTime;

        if (_atkTimer >= _speed)
        {
            NewState(States.Smashing);
        }


        if (!_enemyOnSight)
        {
            _atkTimer = 0;
            NewState(States.Wandering);
        }
    }

    private void Smash()
    {
        if (_canAttack)
        {
            int spawnPosition = 0;

            if (_handIndex == 0)
                spawnPosition = 1;

            _enemySpawner.SpawnEnemy(spawnPosition);

            _atkTimer = 0;
            _hands[_handIndex].transform.DOMove(_handsEndPos[_handIndex].position, _acceleration)
            .SetUpdate(UpdateType.Fixed)
            .SetEase(Ease.InElastic)
            .OnComplete(() => NewState(States.Returning));
        }

        _canAttack = false;
    }

    private void ReturnHand()
    {
        _atkTimer += Time.deltaTime;

        if (_atkTimer >= _speed && !_returning)
        {
            _returning = true;

            _hands[_handIndex].transform.DOMove(_handsStartPos[_handIndex].position, _acceleration)
            .SetUpdate(UpdateType.Fixed)
            .SetEase(Ease.InOutBack)
            .OnComplete(() =>
            {
                _canAttack = true;
                NewState(States.Wandering);
                _returning = false;

                if (_handIndex == 0)
                    _handIndex = 1;
                else
                    _handIndex = 0;
            }
        );
        }
    }

    private void NewState(States newState)
    {
        _currentState = newState;
    }

    /// <Summary>
    /// Activa al boss y le
    /// da una mini animacion
    /// </Summary>
    public void ActivateBoss()
    {
        _uiController.UpdateBossHealth(_currentHP);
        // se puede hacer mas prolijo pero no creo que se modifique tanto
        _transform.DOMove(_bossPoints[0].position, _speed)
        .SetUpdate(UpdateType.Fixed)
        .SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            _transform.DOMove(_bossPoints[1].position, _speed)
            .SetUpdate(UpdateType.Fixed)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => _active = true);
        }
        );
    }

    protected override void PauseEnemy()
    {
        base.PauseEnemy();
        _transform.DOPause();

        // pausa las manos
        foreach (GameObject hand in _hands)
        {
            hand.transform.DOPause();
        }
    }

    protected override void ResumeEnemy()
    {
        base.ResumeEnemy();
        _transform.DOPlay();

        // resume las manos
        foreach (GameObject hand in _hands)
        {
            hand.transform.DOPlay();
        }
    }

    public override void TakeDamage(int value)
    {
        if (!_active) return;

        base.TakeDamage(value);
        _uiController.UpdateBossHealth(_currentHP);
    }

    protected override void OnStartGameOver()
    {
        _active = false;
        NewState(States.Wandering);
        _atkTimer = 0;
        _textState.text = _currentState.ToString();
        base.OnStartGameOver();
    }

    protected override void OnGameOver()
    {
        base.OnGameOver();
        _enemySpawner.DestroyEnemies();

        int index = 0;
        foreach (GameObject hand in _hands)
        {
            hand.transform.position = _handSpawnPos[index];
            index += 1;
        }
    }

    public bool CanAttack
    {
        get { return _canAttack; }
    }

    public bool Active
    {
        get { return _active; }
    }
}
