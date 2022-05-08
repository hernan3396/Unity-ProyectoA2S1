using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    #region Components
    [SerializeField] private UIController _uiController;
    [SerializeField] private PoolManager _bulletPool;
    [SerializeField] private PoolManager _rocketPool;
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

    public Shooting GetShooting
    {
        get { return _shooting; }
    }

    public Camera GetMainCamera
    {
        get { return _mainCamera; }
    }

    public UIController GetUIController
    {
        get { return _uiController; }
    }
}
