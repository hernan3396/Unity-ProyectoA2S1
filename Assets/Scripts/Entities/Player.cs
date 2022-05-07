using UnityEngine;
using StarterAssets;
using System.Collections;

public class Player : MonoBehaviour
{
    #region Components
    private Rigidbody _rb;
    private Inputs _input;
    private Camera _cam;
    private UIController _uiController;
    #endregion

    #region HealthPoints
    [SerializeField] private float _invulnerability = 2;
    [SerializeField] private int _healthPoints = 100;
    private bool _isInmune = false;
    #endregion

    #region BodyParts
    [Header("Body Parts")]
    [SerializeField] private Transform _shootingPos;
    [SerializeField] private Transform _arm;
    private Transform _transform;
    #endregion

    #region Jumping
    [Header("Jumping")]
    [SerializeField] private float _gravityScale = 10f;
    [SerializeField] private int _jumpForce = 15;
    [SerializeField] private float _jumpTime = 1;
    [SerializeField] private float _floorDistance; // distancia para dibujar la caja en el piso
    [SerializeField] private int _speed = 20;
    private bool _isGrounded = true;
    private bool _jumping = false;
    private float _jumpTimer;
    #endregion

    #region Aiming
    [Header("Aiming")]
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private int _bulletSpeed = 20;
    private bool _canShoot = true;
    private Vector3 _aimPosition;
    private bool _isMouse = true;
    #endregion

    private Shooting _shooting;
    [SerializeField] private GameObject _rocket;
    private bool _canCannonShoot = true;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _input = GetComponent<Inputs>();
        _rb = GetComponent<Rigidbody>();

        // log
        _input.OnControlChanged += ControlChanged;
    }

    private void Start()
    {
        _uiController = GameManager.GetInstance.GetUIController;
        _shooting = GameManager.GetInstance.GetShooting;
        _cam = GameManager.GetInstance.GetMainCamera;

        _uiController.UpdateHealthPoints(_healthPoints); // seteo inicial de la UI
    }

    private void Update()
    {
        Aim();
        Move();

        if (_isGrounded && _input.jump)
            Jump();

        if (_jumping)
        {
            _jumpTimer -= Time.deltaTime;

            if (_jumpTimer <= 0 || !_input.jump)
                StopJump();
        }

        if (_canShoot && _input.IsShooting)
            StartCoroutine("Shooting");

        if (_canCannonShoot && _input.CannonShooting)
            StartCoroutine("CannonShooting");
    }

    private void FixedUpdate()
    {
        _rb.AddForce(Physics.gravity * _gravityScale, ForceMode.Acceleration); // simula una gravedad mas pesada

        if (Physics.BoxCast(_transform.position, _transform.localScale / 2, Vector3.down, out RaycastHit hit, Quaternion.identity, _floorDistance))
        {
            _isGrounded = hit.transform.CompareTag("Floor");
            return;
        }

        _isGrounded = false;
    }

    #region HorizontalMovement
    private void Move()
    {
        // Debug.Log(_input.move);
        // _rb.velocity = new Vector3(_input.move.x * _speed, _rb.velocity.y);
    }
    #endregion

    #region Jumping
    private void Jump()
    {
        _jumping = true;
        _jumpTimer = _jumpTime; // timer para limitar el salto
        _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _rb.velocity.z);
    }

    private void StopJump()
    {
        _jumping = false;
        _jumpTimer = 0;
        // _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce * 0.5f, _rb.velocity.z); // pegaba un minisalto cuando lo soltabas 
        _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y * 0.5f, _rb.velocity.z);
    }
    #endregion

    #region Aiming
    private void Aim()
    {
        if (_isMouse)
        {
            Ray ray = _cam.ScreenPointToRay(_input.look);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                _aimPosition = (Vector2)hit.point;

            _aimPosition.z = 0;
            _arm.right = _aimPosition - _arm.position;
        }
        else
        {
            _arm.right = _input.look;
        }
    }

    private IEnumerator Shooting()
    {
        _canShoot = false;

        Vector3 bulletDirection = (_aimPosition - _arm.position).normalized;
        _shooting.Shoot(_shootingPos.position, bulletDirection, _bulletSpeed);
        yield return new WaitForSeconds(_fireRate);

        _canShoot = true;
    }

    // private void CannonShooting()
    // {
    //     // como el sistema de disparos no maneja 2 tipos de balas
    //     // de momento queda aca
    //     GameObject go = Instantiate(_rocket, _shootingPos.position, Quaternion.identity);
    //     Rigidbody rb = go.GetComponent<Rigidbody>();

    //     Vector3 bulletDirection = (_aimPosition - _arm.position).normalized;
    //     rb.AddForce(bulletDirection * _bulletSpeed, ForceMode.Impulse);
    //     // Debug.Log("Pepes");
    // }

    private IEnumerator CannonShooting()
    {
        _canCannonShoot = false;

        GameObject go = Instantiate(_rocket, _shootingPos.position, Quaternion.identity);
        Rigidbody rb = go.GetComponent<Rigidbody>();

        Vector3 bulletDirection = (_aimPosition - _arm.position).normalized;
        rb.AddForce(bulletDirection * _bulletSpeed, ForceMode.Impulse);

        yield return new WaitForSeconds(_fireRate);
        _canCannonShoot = true;
    }
    #endregion

    #region ControlChanged
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
    #endregion

    #region Damage
    public void TakeDamage(int value)
    {
        if (_isInmune) return;
        _isInmune = true;

        _healthPoints -= value;
        _uiController.UpdateHealthPoints(_healthPoints);

        StartCoroutine("InmuneReset");

        if (_healthPoints <= 0)
            Death();
    }

    private IEnumerator InmuneReset()
    {
        yield return new WaitForSeconds(_invulnerability);
        _isInmune = false;
    }

    private void Death()
    {
        GameManager.GetInstance.GameOver();
    }
    #endregion

    private void OnDestroy()
    {
        _input.OnControlChanged -= ControlChanged;
    }
}
