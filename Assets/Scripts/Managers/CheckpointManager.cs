using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private SavesData _savesData;

    private void Awake()
    {
        // _currentCheckpoint = new Vector2(PlayerPrefs.GetFloat("SCC_CheckpointX"), PlayerPrefs.GetFloat("SCC_CheckpointY"));
        // Debug.Log(_currentCheckpoint);
        LoadData();
    }

    private void LoadData()
    {
        _savesData.CurrentCheckpoint = new Vector2(PlayerPrefs.GetFloat("SCC_CheckpointX"), PlayerPrefs.GetFloat("SCC_CheckpointY"));
    }

    public void SetCheckpoint(Transform destination)
    {
        PlayerPrefs.SetFloat("SCC_CheckpointX", destination.position.x);
        PlayerPrefs.SetFloat("SCC_CheckpointY", destination.position.y);
        _savesData.CurrentCheckpoint = destination.position;
    }

    public Vector2 GetCurrentCheckpoint
    {
        get { return _savesData.CurrentCheckpoint; }
    }

    public string GetCurrentLevel
    {
        get { return _savesData.CurrentLevel; }
    }

    public int GetBulletAmount
    {
        get { return _savesData.BulletAmount; }
    }

    public int GetRocketAmount
    {
        get { return _savesData.RocketAmount; }
    }
}