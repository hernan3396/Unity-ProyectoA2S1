using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Vector2 _currentCheckpoint;

    private void Awake()
    {
        _currentCheckpoint = new Vector2(PlayerPrefs.GetFloat("SCC_CheckpointX"), PlayerPrefs.GetFloat("SCC_CheckpointY"));
        Debug.Log(_currentCheckpoint);
    }

    public void SetCheckpoint(Transform destination)
    {
        PlayerPrefs.SetFloat("SCC_CheckpointX", destination.position.x);
        PlayerPrefs.SetFloat("SCC_CheckpointY", destination.position.y);
        _currentCheckpoint = destination.position;
    }

    public Vector2 GetCurrentCheckpoint
    {
        get { return _currentCheckpoint; }
    }
}
