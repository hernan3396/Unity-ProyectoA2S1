using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _debugMenu;

    [SerializeField] private TMP_Text _healthPoints;

    private void Awake()
    {
#if UNITY_EDITOR	
        _debugMenu.SetActive(true);
#endif
    }

    public void UpdateHealthPoints(int value)
    {
        _healthPoints.text = "HP: " + value.ToString();
    }
}
