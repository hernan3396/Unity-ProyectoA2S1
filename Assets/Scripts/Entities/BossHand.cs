using UnityEngine;

public class BossHand : MonoBehaviour
{
    [SerializeField] private BoxCollider[] _handsHitbox;
    [SerializeField] private int _damage;
    [SerializeField] private int _force;
    private Player _player;

    private void Start()
    {
        _player = GameManager.GetInstance.GetPlayerPos.GetComponent<Player>();
    }

    public void PushPlayer()
    {
        foreach (BoxCollider hand in _handsHitbox)
        {
            hand.enabled = false;
        }

        _player.TakeDamage(_damage);

        Rigidbody rb = _player.GetRB;

        Vector3 dir = Vector3.up * _force;
        rb.velocity += dir;
        Invoke("ReturnHands", 0.5f);
    }

    private void ReturnHands()
    {
        foreach (BoxCollider hand in _handsHitbox)
        {
            hand.enabled = true;
        }
    }
}
