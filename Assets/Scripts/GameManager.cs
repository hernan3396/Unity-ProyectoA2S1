using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    #region Components
    [SerializeField] private PoolManager _enemyBulletPool;
    [SerializeField] private Transform _playerPos;
    [SerializeField] private Shooting _shooting;
    #endregion

    [SerializeField] private Camera _mainCamera;

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

    public PoolManager GetEnemyBulletPool
    {
        get { return _enemyBulletPool; }
    }

    public Shooting GetShooting
    {
        get { return _shooting; }
    }

    public Camera GetMainCamera
    {
        get { return _mainCamera; }
    }
}
