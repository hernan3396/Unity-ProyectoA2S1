using UnityEngine;

public class Rocket : MonoBehaviour
{
    #region Parameters
    [Header("Parameters")]
    [SerializeField] private float _explosionDuration;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private int _explosionForce;
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

    private void Update()
    {

    }

    private void Explosion()
    {
        Collider[] hitColliders = Physics.OverlapSphere(_transform.position, _explosionRadius);
        foreach (Collider collider in hitColliders)
        {
            if (collider.TryGetComponent(out Player player))
            {
                // Debug.Log("Aqui");
                // para probar esto hay que deshabilitar el move del player
                Rigidbody rb = player.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.AddForce((player.GetComponent<Transform>().position - transform.position).normalized * _explosionForce, ForceMode.Impulse);
            }
        }
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")) return;

        Explosion();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
