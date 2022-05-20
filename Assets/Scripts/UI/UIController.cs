using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Debug Menu")]
    [SerializeField] private GameObject _debugMenu;

    [Header("Health")]
    [SerializeField] private TMP_Text _healthPoints;

    #region Pause
    [Header("Pause")]
    [SerializeField] private Button _pauseFirstSelected;
    [SerializeField] private GameObject _pauseMenu;
    #endregion

    #region Inventory
    [Header("Inventory")]
    [SerializeField] private TMP_Text[] _itemTxt;
    #endregion

    private void Awake()
    {
#if UNITY_EDITOR
        _debugMenu.SetActive(true);
#endif
    }

    private void Start()
    {
        GameManager.GetInstance.onGamePause += OnGamePaused;
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
    }

    public void UpdateItemText(int itemID, int value)
    {
        _itemTxt[itemID].text = value.ToString();
    }

    private void OnDestroy()
    {
        GameManager.GetInstance.onGamePause -= OnGamePaused;
    }
}
