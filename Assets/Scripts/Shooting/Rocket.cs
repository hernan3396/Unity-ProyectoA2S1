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

    private TrailRenderer _trailRenderer;
    private Transform _transform;
    private Rigidbody _rb;

    private void Awake()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _explosionPool = GameManager.GetInstance.GetExplosionPool;
    }

    private void OnEnable()
    {
        CancelInvoke();
        Invoke("DeactivateBullet", _duration);
    }

    private void Explosion()
    {
        // cuando choca contra mas de un collider llama a este metodo 2 veces
        // no se como solucionarlo D:
        Collider[] hitColliders = Physics.OverlapSphere(_transform.position, _explosionRadius); // ve contra que choca la explosion
        foreach (Collider collider in hitColliders)
        {
            // if (collider.TryGetComponent(out Player player))
            // {
            //     // para probar esto hay que deshabilitar el move del player
            //     Rigidbody rb = player.GetComponent<Rigidbody>();
            //     rb.velocity = Vector3.zero;
            //     rb.AddForce((player.GetComponent<Transform>().position - transform.position).normalized * _explosionForce, ForceMode.Impulse);
            // }

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
                collider.gameObject.GetComponent<Enemy>().TakeDamage(_damage);
        }

        GameObject explosion = _explosionPool.GetPooledObject();
        if (!explosion) return;

        explosion.transform.position = _transform.position;
        explosion.SetActive(true);
        explosion.GetComponent<ParticleSystem>().Play();

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

        Explosion();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
