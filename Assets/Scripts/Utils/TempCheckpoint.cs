using UnityEngine;

public class TempCheckpoint : MonoBehaviour
{
    [SerializeField] private Transform _objective;

    private void Awake()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.transform.position = _objective.position;
    }
}
