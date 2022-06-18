using UnityEngine;
using TMPro;
using DG.Tweening;

public class BossMain : Enemy
{
    [SerializeField] private TMP_Text _textState;

    #region Hands
    [Header("Hands")]
    [SerializeField] private Transform _rHandEndPos;
    [SerializeField] private GameObject _rHand;
    [SerializeField] private GameObject _lHand;
    private Vector3 _rHandStartPos;
    private Vector3 _lHandStartPos;
    #endregion

    [SerializeField] private float _atkTimer;
    private bool _canAttack = true;

    // Arreglar los estados
    // Arreglar los estados
    // Arreglar los estados
    // Arreglar los estados
    // Arreglar los estados
    // Arreglar los estados

    protected override void Awake()
    {
        base.Awake();
        _currentState = States.Wandering;

        _rHandStartPos = _rHand.transform.position;
        _lHandStartPos = _lHand.transform.position;
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
            NewState(States.Smashing);


        if (!_enemyOnSight)
        {
            _atkTimer = 0;
            NewState(States.Wandering);
        }
    }

    private void NewState(States newState)
    {
        _currentState = newState;
    }

    private void Smash()
    {
        if (_canAttack)
        {
            _atkTimer = 0;
            _rHand.transform.DOMove(_rHandEndPos.position, _acceleration)
            .SetEase(Ease.InElastic)
            .OnComplete(() => NewState(States.Returning));
        }

        _canAttack = false;
    }

    private void ReturnHand()
    {
        _atkTimer += Time.deltaTime;

        if (_atkTimer >= _speed)
            _rHand.transform.DOMove(_rHandStartPos, _acceleration)
            .SetEase(Ease.InOutBack)
            .OnComplete(() =>
            {
                _canAttack = true;
                NewState(States.Wandering);
            });
    }
}
