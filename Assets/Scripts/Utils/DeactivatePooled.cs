using UnityEngine;

public class DeactivatePooled : MonoBehaviour
{
    [SerializeField] private float _duration;
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        _particleSystem.Play();

        CancelInvoke();
        Invoke("Deactivate", _duration);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
