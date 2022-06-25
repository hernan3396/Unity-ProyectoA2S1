using UnityEngine;
using Cinemachine;

public class ChangeCameras : MonoBehaviour
{
    [SerializeField] private GameObject _mainCamBrain;
    private GameObject _activeCamera;

    private void Start()
    {
        GameManager.GetInstance.onGameOver += ActivateMain;
    }

    public void ActivateNextCam(GameObject nextCam)
    {
        _mainCamBrain.SetActive(false);
        _activeCamera = nextCam;
        _activeCamera.SetActive(true);

    }

    public void ActivateMain()
    {
        if (_activeCamera)
            _activeCamera.SetActive(false);

        _mainCamBrain.SetActive(true);
    }

    private void OnDestroy()
    {
        GameManager.GetInstance.onGameOver += ActivateMain;
    }
}
