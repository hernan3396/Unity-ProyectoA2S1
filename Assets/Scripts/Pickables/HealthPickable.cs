using UnityEngine;

public class HealthPickable : MonoBehaviour
{
    #region Paremeters
    [Header("Parameters")]
    [SerializeField] private int _curation;
    #endregion

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void DesactivateHealth()
    {
        _rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Debug.Log("agarrado");
            other.gameObject.GetComponent<Player>().AddHealth(_curation);
            DesactivateHealth();
            return;
        }

        if (other.gameObject.CompareTag("Floor"))
        {
            _rb.velocity = Vector3.zero;
            return;
        }
    }
}
