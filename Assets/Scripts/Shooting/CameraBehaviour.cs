using UnityEngine;
using Cinemachine;

public class CameraBehaviour : MonoBehaviour
{
    #region Components
    private CinemachineCameraOffset _vCameraOffset;
    private Transform _playerPos;
    private Inputs _input;
    private Camera _cam;
    #endregion

    [SerializeField] private int _cameraDeadZone;
    [SerializeField] private int _offsetDistance;
    [SerializeField] private int _cameraSpeed;
    private Vector3 _initialCamOffset;
    private bool _isMouse = true;
    private Vector2 aimPosition;
    private Vector2 _direction;

    private void Awake()
    {
        _vCameraOffset = GetComponent<CinemachineCameraOffset>();
        _initialCamOffset = _vCameraOffset.m_Offset;
    }

    private void Start()
    {
        _playerPos = GameManager.GetInstance.GetPlayerPos;
        _cam = GameManager.GetInstance.GetMainCamera;
        _input = GameManager.GetInstance.GetInput;

        _input.OnControlChanged += ControlChanged;
    }

    private void LateUpdate()
    {
        if (_isMouse)
        {
            Ray ray = _cam.ScreenPointToRay(_input.look);


            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                aimPosition = (Vector2)hit.point;

            _direction = (aimPosition - (Vector2)_playerPos.position).normalized;
        }
        else
        {
            // control
            _direction = _input.look;
        }


        // si la magnitud <= 0.1 -> ponerle el valor inicial!!
        // no pude usar _direction porque ya es un vector normalizado (y su magnitud siempre es 1)
        // con el control puede ser que de algun problema pero de momento no ha pasado
        if ((aimPosition - (Vector2)_playerPos.position).magnitude < _cameraDeadZone)
            _vCameraOffset.m_Offset = Vector3.Slerp(_vCameraOffset.m_Offset, _initialCamOffset, Time.deltaTime * _cameraSpeed);
        else
            _vCameraOffset.m_Offset = Vector3.Slerp(_vCameraOffset.m_Offset, _direction * _offsetDistance, Time.deltaTime * _cameraSpeed);
    }

    private void ControlChanged(string value)
    {
        // DualShock4GamepadHID nombre del control de ps4
        switch (value)
        {
            case "Keyboard":
                _isMouse = true;
                break;
            default:
                _isMouse = false;
                break;
        }
    }

    private void OnDestroy()
    {
        _input.OnControlChanged -= ControlChanged;
    }
}
