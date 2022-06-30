using System.Diagnostics;
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

    [SerializeField] private GameObject _bossHealth;
    [SerializeField] private GameObject _bossLifebar1;
    [SerializeField] private GameObject _bossLifebar2;
    [SerializeField] private GameObject _bossLifebar3;
    [SerializeField] private GameObject _bossLifebar4;

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
        _lifeBar.GetComponent<Slider>().value = value + 34;
    }

    public void UpdateItemText(int itemID, int value)
    {
        _itemTxt[itemID].text = value.ToString();
    }

    public void UpdateState(string newState)
    {
        _stateText.text = $"State: {newState}";
    }

    // public void UpdateBossHealth(int value)
    // {
    //     switch (value)
    //     {
    //         case 1:
    //             _bossLifebar1.SetActive(false);
    //             break;
    //         case 2:
    //             _bossLifebar2.SetActive(false);
    //             break;
    //         case 3:
    //             _bossLifebar3.SetActive(false);
    //             break;
    //         case 4:
    //             _bossLifebar4.SetActive(false);
    //             break;
    //     }
    // }

    public void UpdateWakePointHealth(int value, int lifeBar)
    {
        if (!_bossHealth) return;
        switch (lifeBar)
        {
            case 1:
                _bossLifebar1.GetComponent<Slider>().value = value;
                break;
            case 2:
                _bossLifebar2.GetComponent<Slider>().value = value;
                break;
            case 3:
                _bossLifebar3.GetComponent<Slider>().value = value;
                break;
            case 4:
                _bossLifebar4.GetComponent<Slider>().value = value;
                break;
        }
    }

    public void OnBossDeath()
    {
        _bossHealth.SetActive(false);
    }

    private void OnStartGameOver()
    {
        _deathScreen.SetActive(true);
        if (_bossHealth)
            _bossHealth.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.GetInstance.onGamePause -= OnGamePaused;
        GameManager.GetInstance.onStartGameOver -= OnStartGameOver;
    }
}