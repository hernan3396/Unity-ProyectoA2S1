using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    #region Parameters
    [Header("Parameters")]
    [SerializeField] private EnemyData _enemyData;
    // parece que estan medios desordenados pero es el
    // orden que estan en el scriptable object
    // asi mantenemos coherencia entre ambos
    private int _hp;
    private int _damage;
    private float _fireRate;
    private int _movSpeed;
    private float _accuracy;
    #endregion

    #region BodyParts
    [Header("Body Parts")]
    [SerializeField] private Transform _arm;
    private Transform _transform;
    #endregion

    private Transform _targetPos;

    private void Awake()
    {
        SetStats();
    }

    private void Start()
    {
        _targetPos = GameManager.GetInstance.GetPlayerPos;
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        Aim();
    }

    private void SetStats()
    {
        _hp = _enemyData.HP;
        _damage = _enemyData.DAMAGE;
        _fireRate = _enemyData.FIRERATE;
        _movSpeed = _enemyData.MOVSPEED;
        _accuracy = _enemyData.ACCURACY;
    }

    private void Aim()
    {
        Vector3 dir = _targetPos.position - _transform.position;

        DOTween.To(() => _arm.right, x => _arm.right = x, dir, _accuracy);
        // _arm.right = test;
    }
}
