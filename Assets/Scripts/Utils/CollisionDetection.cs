using UnityEngine;
using UnityEngine.Events;

public class CollisionDetection : MonoBehaviour
{
    // intento de script generico para colisiones
    public UnityEvent onEnter;
    public UnityEvent onExit;

    [SerializeField] private bool _useTrigger;
    // no se si es necesario este chequeo pero
    // no esta de mas hacerlo (?)
    [SerializeField] private string _tag;

    #region TriggerEnter
    private void OnTriggerEnter(Collider other)
    {
        if (!_useTrigger) return;
        if (other.gameObject.CompareTag(_tag))
            onEnter?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_useTrigger) return;
        if (other.gameObject.CompareTag(_tag))
            onExit?.Invoke();
    }
    #endregion

    #region CollisionEnter    
    private void OnCollisionEnter(Collision other)
    {
        if (_useTrigger) return;
        if (other.gameObject.CompareTag(_tag))
            onEnter?.Invoke();
    }

    private void OnCollisionExit(Collision other)
    {
        if (_useTrigger) return;
        if (other.gameObject.CompareTag(_tag))
            onExit?.Invoke();
    }
    #endregion
}
