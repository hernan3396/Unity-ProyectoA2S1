using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _firstSelectedButton;

    private void Awake()
    {
        _firstSelectedButton.Select();
    }
}
