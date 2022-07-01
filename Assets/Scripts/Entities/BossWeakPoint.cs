using UnityEngine;
using UnityEngine.Events;

public class BossWeakPoint : Enemy
{
    [SerializeField] private BossMain _bossMain;
    [SerializeField] private int _lifebarNum;
    private UIController _uiController;
    public UnityEvent OnDeath;

    protected override void Start() {
         _uiController = GameManager.GetInstance.GetUIController;
         _uiController.UpdateWakePointHealth(_currentHP, _lifebarNum);
    }

    public override void TakeDamage(int value)
    {
        if (_isInmune) return;
        if (_isDead) return;
        if (!_bossMain.Active) return;

        _isInmune = true;
        StartCoroutine("InmuneReset");

        _currentHP -= value;
        _uiController.UpdateWakePointHealth(_currentHP, _lifebarNum);

        if (_currentHP <= 0)
        {
            _isDead = true;
            OnDeath?.Invoke();
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    protected override void OnStartGameOver()
    {
        base.OnStartGameOver();
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
        return;
    }

    protected override void OnGameOver()
    {
        _currentHP = _hp;
        _isDead = false;
        return;
    }
}
