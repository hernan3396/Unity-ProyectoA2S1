using UnityEngine;

public class ExplosionParticles : MonoBehaviour
{
    [SerializeField] private float _duration;

    private void OnEnable()
    {
        CancelInvoke();
        Invoke("DeactivateExplosion", _duration);
    }

    private void DeactivateExplosion()
    {
        gameObject.SetActive(false);
    }
}
