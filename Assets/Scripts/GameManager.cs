using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    #region Components
    #region Pools
    [Header("Pools")]
    // esto despues se puede pasar a un array de poolmanagers
    // y usar un enum para mantener el orden
    [SerializeField] private PoolManager _enemiesBulletPool;
    [SerializeField] private PoolManager _playerBulletPool;
    [SerializeField] private PoolManager _explosionPool;
    [SerializeField] private PoolManager _rocketPool;
    [SerializeField] private PoolManager _healthPool;
    [SerializeField] private PoolManager _ammoPool;
    [SerializeField] private PoolManager _bloodPool;
    [SerializeField] private PoolManager _dustPool;
    [SerializeField] private PoolManager _sparkPool;
    #endregion

    [Header("TFW no pool")]
    [SerializeField] private Transform _playerObjectivePos;
    [SerializeField] private CameraBehaviour _cameraScript;
    [SerializeField] private InventoryManager _invManager;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private UIController _uiController;
    [SerializeField] private SavesManager _savesManager;
    [SerializeField] private Transform _playerPos;
    [SerializeField] private Shooting _shooting;
    [SerializeField] private Inputs _input;
    #endregion

    #region Pause
    public delegate void OnGamePause(bool isGamePaused);
    public event OnGamePause onGamePause;
    private bool _isPaused = false;
    #endregion

    #region GameOver
    [Header("Game Over")]
    [SerializeField] private float _deathDuration;

    public delegate void OnGameOver();
    public event OnGameOver onGameOver;

    public delegate void OnStartGameOver();
    public event OnStartGameOver onStartGameOver;
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

    public void PauseGame()
    {
        // de momento solo hace aparecer a un panel
        // luego pausaria a las entidades
        _isPaused = !_isPaused;

        if (onGamePause != null)
            onGamePause(_isPaused);
    }

    public void GameOver()
    {
        if (onGameOver != null)
            onGameOver();
    }

    public void StartGameOver()
    {
        if (onStartGameOver != null)
            onStartGameOver();

        Invoke("GameOver", _deathDuration);
    }

    public void QuitGame()
    {
        Application.Quit();
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

    public PoolManager GetPlayerBullet
    {
        get { return _playerBulletPool; }
    }

    public PoolManager GetEnemyBullet
    {
        get { return _enemiesBulletPool; }
    }

    public PoolManager GetRocketPool
    {
        get { return _rocketPool; }
    }

    public PoolManager GetExplosionPool
    {
        get { return _explosionPool; }
    }

    public PoolManager GetHealthPool
    {
        get { return _healthPool; }
    }

    public PoolManager GetAmmoPool
    {
        get { return _ammoPool; }
    }

    public PoolManager GetDustPool
    {
        get { return _dustPool; }
    }

    public PoolManager GetSparkPool
    {
        get { return _sparkPool; }
    }

    public PoolManager GetBloodPool
    {
        get { return _bloodPool; }
    }

    public Shooting GetShooting
    {
        get { return _shooting; }
    }

    public Camera GetMainCamera
    {
        get { return _mainCamera; }
    }

    public Inputs GetInput
    {
        get { return _input; }
    }

    public CameraBehaviour GetCameraBehaviour
    {
        get { return _cameraScript; }
    }

    public UIController GetUIController
    {
        get { return _uiController; }
    }

    public SavesManager GetSavesManager
    {
        get { return _savesManager; }
    }

    public InventoryManager GetInvManager
    {
        get { return _invManager; }
    }

    public Transform PlayerObjPos
    {
        get { return _playerObjectivePos; }
    }
}
