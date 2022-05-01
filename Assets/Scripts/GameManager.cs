using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    #region Components
    [SerializeField] private Transform _playerPos;
    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void OnDestroy()
    {
        if (_instance != this)
        {
            _instance = null;
        }
    }

    public static GameManager GetInstance
    {
        get { return _instance; }
    }

    public Transform GetPlayerPos
    {
        get { return _playerPos; }
    }
}
