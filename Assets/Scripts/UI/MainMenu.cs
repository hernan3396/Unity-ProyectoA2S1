using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _firstSelectedButton;
    [SerializeField] private Button _loadGameButton;
    private SavesManager _savesManager;

    private void Awake()
    {
        _firstSelectedButton.Select();
    }

    private void Start()
    {
        _savesManager = GameManager.GetInstance.GetSavesManager;

        if (_savesManager.GetCurrentLevel == "MainMenu")
            _loadGameButton.interactable = false;
    }
}
