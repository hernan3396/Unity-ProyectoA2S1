using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Components
    private PoolManager _sparkPool;
    #endregion

    #region Paremeters
    [Header("Parameters")]
    [SerializeField] private float _duration;
    [SerializeField] private int _damage;
    #endregion

    #region Pause
    private Vector2 _lastVelocity;
    private bool _isPaused;
    #endregion

    private TrailRenderer _trailRenderer;
    private Rigidbody _rb;

    private void Awake()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _sparkPool = GameManager.GetInstance.GetSparkPool;

        GameManager.GetInstance.onGamePause += OnPause;
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
            other.gameObject.GetComponent<Enemy>().SetMeleeDamage = false;
            DeactivateBullet();
            return;
        }

        if (other.gameObject.CompareTag("Floor"))
        {
            DeactivateBullet();
            return;
        }

        if (other.gameObject.CompareTag("Armor"))
        {
            GameObject spark = _sparkPool.GetPooledObject();
            if (!spark) return;

            spark.transform.position = transform.position;
            spark.SetActive(true);
            spark.GetComponent<ParticleSystem>().Play();
            return;
        }

        // gameObject.SetActive(false); // si no choca contra nada
    }
}