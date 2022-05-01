using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _debugMenu;

    private void Awake()
    {
#if UNITY_EDITOR	
        _debugMenu.SetActive(true);
#endif
    }
}
