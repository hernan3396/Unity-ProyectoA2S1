using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Debug Menu")]
    [SerializeField] private GameObject _debugMenu;
    [SerializeField] private TMP_Text _stateText;

    [Header("Health")]
    [SerializeField] private TMP_Text _healthPoints;
    [SerializeField] private GameObject _lifeBar;

    #region Pause
    [Header("Pause")]
    [SerializeField] private Button _pauseFirstSelected;
    [SerializeField] private GameObject _pauseMenu;
    #endregion

    #region Inventory
    [Header("Inventory")]
    [SerializeField] private TMP_Text[] _itemTxt;
    #endregion

    #region DeathScreen
    [Header("Death Screen")]
    [SerializeField] private GameObject _deathScreen;
    #endregion

    [SerializeField] private TMP_Text _bossHealth;

    private void Awake()
    {
#if UNITY_EDITOR
        _debugMenu.SetActive(true);
#endif
    }

    private void Start()
    {
        GameManager.GetInstance.onGamePause += OnGamePaused;
        GameManager.GetInstance.onStartGameOver += OnStartGameOver;
    }

    private void OnGamePaused(bool isPaused)
    {
        _pauseMenu.SetActive(isPaused);

        if (isPaused)
            _pauseFirstSelected.Select();
    }

    public void UpdateHealthPoints(int value)
    {
        _healthPoints.text = "HP: " + value.ToString();
        _lifeBar.GetComponent<Slider>().value = value;
    }

    public void UpdateItemText(int itemID, int value)
    {
        _itemTxt[itemID].text = value.ToString();
    }

    public void UpdateState(string newState)
    {
        _stateText.text = $"State: {newState}";
    }

    public void UpdateBossHealth(int value)
    {
        _bossHealth.text = $"Boss HP: {value}";
    }

    private void OnStartGameOver()
    {
        _deathScreen.SetActive(true);
        if (_bossHealth)
            _bossHealth.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.GetInstance.onGamePause -= OnGamePaused;
        GameManager.GetInstance.onStartGameOver -= OnStartGameOver;
    }
}