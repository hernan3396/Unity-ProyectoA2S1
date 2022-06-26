using UnityEngine;
using DG.Tweening;

public class ShockWave : MonoBehaviour
{
    [SerializeField] private Vector3 _endPos;
    [SerializeField] private float _duration;
    [SerializeField] private int _force;
    [SerializeField] private int _damage;
    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Start()
    {
        _transform.DOMove((_transform.position + _endPos), _duration)
        .SetUpdate(UpdateType.Fixed)
        .SetEase(Ease.InQuint)
        .OnComplete(() =>
        {
            Destroy(gameObject);
        }
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponentInParent<Player>();
            player.TakeDamage(_damage);

            Rigidbody rb = player.GetRB;
            // rb.velocity = Vector3.zero;
            Vector3 dir = (other.transform.position - _transform.position).normalized;
            player.RocketJumping(true);
            rb.velocity += dir * _force * 0.75f; // añadis la velocidad de la explosion
        }

        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponentInParent<Enemy>();

            enemy.TakeDamage(40);

            Rigidbody rb = other.GetComponentInChildren<Rigidbody>();

            if (rb)
            {
                rb.velocity = Vector3.zero;

                Vector3 dir = (other.transform.position - _transform.position).normalized;
                rb.velocity = new Vector3(-dir.x * _force, dir.y * _force, 0) * 2;
            }
        }
    }
}
