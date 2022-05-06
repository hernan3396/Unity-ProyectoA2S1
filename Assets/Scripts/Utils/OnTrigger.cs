using UnityEngine;
using UnityEngine.Events;

public class OnTrigger : MonoBehaviour
{
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
    [SerializeField] private string otherTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(otherTag))
        {
            onTriggerEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(otherTag))
        {
            onTriggerExit?.Invoke();
        }
    }

    public void Test()
    {
        Debug.Log("Algo");
    }
}
