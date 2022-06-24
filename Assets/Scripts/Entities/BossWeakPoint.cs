using UnityEngine;
using UnityEngine.Events;

public class BossWeakPoint : Enemy
{
    [SerializeField] private BossMain _bossMain;
    public UnityEvent OnDeath;

    public override void TakeDamage(int value)
    {
        if (_isInmune) return;
        if (_isDead) return;
        if (!_bossMain.Active) return;

        _isInmune = true;
        StartCoroutine("InmuneReset");

        _currentHP -= value;

        if (_currentHP <= 0)
        {
            _isDead = true;
            OnDeath?.Invoke();
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    protected override void OnStartGameOver()
    {
        GetComponent<MeshRenderer>().enabled = true;
        return;
    }

    protected override void OnGameOver()
    {
        _currentHP = _hp;
        _isDead = false;
        return;
    }
}
