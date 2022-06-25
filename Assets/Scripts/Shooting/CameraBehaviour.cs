using UnityEngine;
using Cinemachine;

public class CameraBehaviour : MonoBehaviour
{
    #region Components
    private CinemachineCameraOffset _vCameraOffset;
    private CinemachineBasicMultiChannelPerlin _vCameraNoise; // Para el Shake
    private Transform _playerPos;
    private Inputs _input;
    private Camera _cam;
    #endregion

    #region Settings
    [SerializeField] private int _cameraDeadZone;
    [SerializeField] private int _offsetDistance;
    [SerializeField] private int _cameraSpeed;
    private float _shakeTime = 0;
    private float _totalShakeTime;
    private float _shakeIntensity;
    private Vector3 _initialCamOffset;
    private bool _isMouse = true;
    private Vector2 aimPosition;
    private Vector2 _direction;
    #endregion

    private Vector3 _camOffset;

    private void Awake()
    {
        _vCameraOffset = GetComponent<CinemachineCameraOffset>();
        _initialCamOffset = _vCameraOffset.m_Offset;
        _vCameraNoise = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>(); //Lo que hace el shake
    }

    private void Start()
    {
        _playerPos = GameManager.GetInstance.GetPlayerPos;
        _cam = GameManager.GetInstance.GetMainCamera;
        _input = GameManager.GetInstance.GetInput;

        _input.OnControlChanged += ControlChanged;
    }

    private void Update()
    {
        if (_shakeTime > 0)
        {
            _shakeTime -= Time.deltaTime;
            StopShake(_shakeIntensity, _shakeTime);
        }
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
        Vector3 aimDir = (aimPosition - (Vector2)_playerPos.position);
        _camOffset = _initialCamOffset;

        if (aimDir.magnitude > _cameraDeadZone)
            _camOffset = _direction * _offsetDistance;

        _vCameraOffset.m_Offset = Vector3.Slerp(_vCameraOffset.m_Offset, _camOffset, Time.deltaTime * _cameraSpeed);
    }

    public void ShakeCamera(float intensity, float time)
    {
        _vCameraNoise.m_AmplitudeGain = intensity;
        _totalShakeTime = _shakeTime = time;
        _shakeIntensity = intensity;
    }

    private void StopShake(float intensity, float time)
    {
        _vCameraNoise.m_AmplitudeGain = Mathf.Lerp(intensity, 0f, 1 - (time / _totalShakeTime));
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

    public Vector3 CamOffset
    {
        get { return _camOffset; }
    }

    public Vector2 GetDirection
    {
        get { return _direction; }
    }
}
