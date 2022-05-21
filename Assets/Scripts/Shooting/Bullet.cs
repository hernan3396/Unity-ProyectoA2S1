using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Paremeters
    [Header("Parameters")]
    [SerializeField] private float _duration;
    [SerializeField] private int _damage;
    #endregion

    private TrailRenderer _trailRenderer;
    private Rigidbody _rb;

    private void Awake()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        CancelInvoke();
        Invoke("DeactivateBullet", _duration);
    }

    private void DeactivateBullet()
    {
        _trailRenderer.Clear();
        _rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        // sabemos que no es lo mejor pero de momento funciona

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().TakeDamage(_damage);
            DeactivateBullet();
            return;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(_damage);
            DeactivateBullet();
            return;
        }

        if (other.gameObject.CompareTag("Floor"))
        {
            DeactivateBullet();
            return;
        }

        if (other.gameObject.CompareTag("BounceWall"))
        {
            Vector3 inNormal = other.contacts[0].normal; // la normal del plano
            Vector3 direction = transform.position - other.contacts[0].point;

            Debug.Log("punto de choque" + other.contacts[0].point);
            Debug.Log("transform bala" + transform.position);

            Vector3 outDir = Vector3.Reflect(direction.normalized, inNormal);
            _rb.velocity = outDir.normalized * 30;
            return;
        }

        // gameObject.SetActive(false); // si no choca contra nada
    }
}