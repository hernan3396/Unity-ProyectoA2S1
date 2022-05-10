using UnityEngine;

public class Rocket : MonoBehaviour
{
    #region Parameters
    [Header("Parameters")]
    [SerializeField] private float _explosionDuration;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private int _explosionForce;
    [SerializeField] private float _duration;
    [SerializeField] private float _damage;
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
                // para probar esto hay que deshabilitar el move del player
                // Debug.Log(collider.name);

                // cambiar esto
                collider.GetComponent<Player>()._isRocketJumping = true;
                collider.GetComponent<Player>()._rocketTimer = collider.GetComponent<Player>()._rocketJumpingTimer;


                Rigidbody rb = collider.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                // rb.AddForce((collider.transform.position - _transform.position).normalized * _explosionForce, ForceMode.Impulse);
                rb.velocity += (collider.transform.position - _transform.position).normalized * _explosionForce;
            }
        }
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
