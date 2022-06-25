using UnityEngine;

public class Crosshair : MonoBehaviour
{
    // un script rapidito que hice para tener un crosshair
    private Camera _cam;
    private Inputs _input;
    private bool _isMouse = true;

    [SerializeField] private Transform _aimDebugSphere;
    [SerializeField] private RectTransform _crosshair;
    [SerializeField] private RectTransform _parent;

    private void Awake()
    {
        Cursor.visible = false;
        _crosshair.gameObject.SetActive(true);
    }

    private void Start()
    {
        _cam = GameManager.GetInstance.GetMainCamera;
        _input = GameManager.GetInstance.GetInput;

        _input.OnControlChanged += ControlChanged;
    }

    // Update is called once per frame
    void Update()
    {
        // peligro
        // fede
        if (_isMouse)
        {
            Vector2 anchoredPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_parent, _input.look, _cam, out anchoredPos);

            _crosshair.anchoredPosition = anchoredPos;
        }
        else
        {
            Vector2 aimScreenPos;
            aimScreenPos = _cam.WorldToScreenPoint(_aimDebugSphere.position);

            Vector2 anchoredPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_parent, aimScreenPos, _cam, out anchoredPos);
            _crosshair.anchoredPosition = anchoredPos;
        }
    }

    //     // peligro
    // // fede
    // Vector2 aimScreenPos;
    // aimScreenPos = _cam.WorldToScreenPoint(_aimDebugSphere.position);

    // Vector2 anchoredPos;
    // RectTransformUtility.ScreenPointToLocalPointInRectangle(_parent, aimScreenPos, _cam, out anchoredPos);
    // _crosshair.anchoredPosition = anchoredPos;
    // // _crosshair.anchoredPosition = aimScreenPos;

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
