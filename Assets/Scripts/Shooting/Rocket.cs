using UnityEngine;

public class Rocket : MonoBehaviour
{
    #region Components
    private PoolManager _explosionPool;
    #endregion

    #region Parameters
    [Header("Parameters")]
    [SerializeField] private float _explosionDuration;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private int _explosionForce;
    [SerializeField] private float _duration;
    [SerializeField] private int _damage;
    #endregion

    #region Pause
    private Vector2 _lastVelocity;
    private bool _isPaused;
    #endregion

    private TrailRenderer _trailRenderer;
    private ExternalSound _extSound;
    private Transform _transform;
    private Rigidbody _rb;

    private void Awake()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
        _extSound = GetComponent<ExternalSound>();
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _explosionPool = GameManager.GetInstance.GetExplosionPool;
        GameManager.GetInstance.onGamePause += OnPause;
    }

    private void OnEnable()
    {
        CancelInvoke();
        Invoke("DeactivateBullet", _duration);
    }

    private void Explosion()
    {
        // toda esta parte seria mas facil con un utils.cs
        // que tenga los pedazos de codigo que se
        // repiten pero bueno...

        // cuando choca contra mas de un collider llama a este metodo 2 veces
        // no se como solucionarlo D:
        Collider[] hitColliders = Physics.OverlapSphere(_transform.position, _explosionRadius); // ve contra que choca la explosion
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                // Debug.Log(collider.name);
                // Player player = collider.gameObject.GetComponent<Player>();
                // no se si es lo correcto
                Player player = collider.gameObject.GetComponentInParent<Player>();
                player.RocketJumping(true);
                player.TakeDamage(_damage / 8); // _damage es (o era) 40 y lo divide para hacerle solo 5 de daño al player
                // mover esto al player??

                Rigidbody rb = player.GetRB;
                // seteas la velocidad en cero para que
                // la actual no se reste con la nueva
                // esto es preferencia pero me gusta mas asi
                rb.velocity = Vector3.zero;
                // calculas la direccion de la explosion
                Vector3 dir = (collider.transform.position - _transform.position).normalized;
                rb.velocity += dir * _explosionForce; // añadis la velocidad de la explosion
            }

            if (collider.gameObject.CompareTag("Enemy"))
            {
                collider.gameObject.GetComponent<Enemy>().TakeDamage(_damage);
                collider.gameObject.GetComponent<Enemy>().SetMeleeDamage = false;

                Rigidbody rb = collider.GetComponentInChildren<Rigidbody>();
                rb.velocity = Vector3.zero;

                Vector3 dir = (collider.transform.position - _transform.position).normalized;
                rb.velocity += dir * _explosionForce * 2;
            }

            if (collider.gameObject.CompareTag("Ammo"))
            {
                Vector2 otherPos = collider.transform.position;
                Rigidbody otherRb;

                if (collider.TryGetComponent(out Rigidbody rb))
                {
                    otherRb = rb;
                    Vector2 direction = otherPos - (Vector2)_transform.position;

                    rb.AddForce(direction.normalized * _explosionForce, ForceMode.Impulse);
                }
            }
        }

        GameObject explosion = _explosionPool.GetPooledObject();
        if (!explosion) return;

        explosion.transform.position = _transform.position;
        explosion.SetActive(true);
        explosion.GetComponent<ParticleSystem>().Play();

        _extSound.PlaySFX();
        DeactivateBullet();
    }

    private void DeactivateBullet()
    {
        _trailRenderer.Clear();
        _rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")) return; // evita que explote al dispararla

        if (other.gameObject.CompareTag("Bullet")) return;

        Explosion();
    }

    #region Pause
    private void OnPause(bool value)
    {
        if (value)
        {
            _lastVelocity = _rb.velocity;
            _rb.velocity = Vector2.zero;
            return;
        }

        _rb.velocity = _lastVelocity;
    }
    #endregion

    private void OnDestroy()
    {
        GameManager.GetInstance.onGamePause -= OnPause;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
