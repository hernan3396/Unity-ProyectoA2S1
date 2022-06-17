using UnityEngine;

public class ChangeCameras : MonoBehaviour
{
    [SerializeField] private GameObject _mainCamBrain;
    private GameObject _activeCamera;

    public void ActivateNextCam(GameObject nextCam)
    {
        _mainCamBrain.SetActive(false);
        _activeCamera = nextCam;
        _activeCamera.SetActive(true);
    }

    public void ActivateMain()
    {
        _activeCamera.SetActive(false);
        _mainCamBrain.SetActive(true);
    }
}
