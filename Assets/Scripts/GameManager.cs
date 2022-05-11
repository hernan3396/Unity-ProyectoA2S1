using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    #region Components
    [SerializeField] private UIController _uiController;
    [SerializeField] private PoolManager _bulletPool;
    [SerializeField] private PoolManager _rocketPool;
    [SerializeField] private PoolManager _healthPool;
    [SerializeField] private Transform _playerPos;
    [SerializeField] private Shooting _shooting;
    [SerializeField] private Inputs _input;
    #endregion

    #region Pause
    public delegate void OnGamePause(bool isGamePaused);
    public event OnGamePause onGamePause;
    private bool _isPaused = false;
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

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("HernanScene"); // cambiar luego
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

    public PoolManager GetBulletPool
    {
        get { return _bulletPool; }
    }

    public PoolManager GetRocketPool
    {
        get { return _rocketPool; }
    }

    public PoolManager GetHealthPool
    {
        get { return _healthPool; }
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

    public UIController GetUIController
    {
        get { return _uiController; }
    }
}
