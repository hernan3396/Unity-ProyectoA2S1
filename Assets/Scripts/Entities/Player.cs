using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Player : MonoBehaviour
{
    #region Components
    private UIController _uiController;
    private Shooting _shooting;
    private Rigidbody _rb;
    private Inputs _input;
    private Camera _cam;
    #endregion

    #region HealthPoints
    [SerializeField] private float _invulnerability = 2;
    [SerializeField] private int _healthPoints = 100;
    private bool _isInmune = false;
    #endregion

    #region BodyParts
    [Header("Body Parts")]
    [SerializeField] private Transform _shootingPos;
    [SerializeField] private Transform _meleeArm;
    [SerializeField] private Transform _arm;
    private Transform _transform;
    #endregion

    #region Jumping
    [Header("Jumping")]
    [SerializeField] private float _heightModifier = 2; // para la altura de la caja que detecta si esta o no en el piso
    [SerializeField] private float _gravityScale = 10f;
    [SerializeField] private int _jumpForce = 15;
    [SerializeField] private float _jumpTime = 1;
    [SerializeField] private float _floorDistance; // distancia para dibujar la caja en el piso
    [SerializeField] private int _speed = 20;
    private bool _isGrounded = true;
    private bool _jumping = false;
    private float _jumpTimer;
    #endregion

    #region RocketJumping
    [Header("Rocket Jumping")]
    [SerializeField] private float _rocketJumpingTimer = 1; // el tiempo maximo que pasa en el estado de "Rocket jumping"
    private bool _isRocketJumping = false;
    private float _rocketTimer;
    #endregion

    #region Aiming
    [Header("Aiming")]
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private int _bulletSpeed = 20;
    private bool _canShoot = true;
    private Vector3 _aimPosition;
    private bool _isMouse = true; // para ver que tipo de input estas usando
    #endregion

    #region Melee
    [Header("Melee")]
    // esta parte se va a ver cambiada cuando tengamos la animacion
    // lo mas seguro es que lo hagamos desde ahi
    [SerializeField] private Vector3 _meleeFinalPos; // el angulo final
    [SerializeField] private float _meleeSpeed = 1; // el tiempo que demora en "Hacer" la animacion del melee
    private Quaternion _meleeInitialRot; // el angulo del que empieza
    private bool _canMelee = true;
    #endregion

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _input = GetComponent<Inputs>();
        _rb = GetComponent<Rigidbody>();

        _meleeInitialRot = _meleeArm.rotation;
    }

    private void Start()
    {
        _uiController = GameManager.GetInstance.GetUIController;
        _shooting = GameManager.GetInstance.GetShooting;
        _cam = GameManager.GetInstance.GetMainCamera;
        _input = GameManager.GetInstance.GetInput;

        _uiController.UpdateHealthPoints(_healthPoints); // seteo inicial de la UI
        _input.OnControlChanged += ControlChanged;
    }

    private void Update()
    {
        Aim();

        if (_isGrounded && _input.jump)
            Jump();

        if (_jumping)
        {
            _jumpTimer -= Time.deltaTime;

            if (_jumpTimer <= 0 || !_input.jump)
                StopJump();
        }

        if (_isRocketJumping)
        {
            _rocketTimer -= Time.deltaTime;

            if (_rocketTimer <= 0)
                RocketJumping(false);
        }

        // aca realmente deberiamos tener una variable
        // que tenga la info de arma seleccionada
        if (_canShoot && _input.IsShooting)
            StartCoroutine("Shoot", (int)Shooting.BulletType.BULLETPOOL);

        if (_canShoot && _input.CannonShooting)
            StartCoroutine("Shoot", (int)Shooting.BulletType.ROCKETPOOL);

        if (_canMelee && _input.Melee)
            StartCoroutine("Melee");
    }

    private void FixedUpdate()
    {
        Move();

        _rb.AddForce(Physics.gravity * _gravityScale, ForceMode.Acceleration); // simula una gravedad mas pesada

        // este cambiarlo luego con un collider pero de momento funciona
        if (Physics.BoxCast(_transform.position, _transform.localScale / 2, Vector3.down, out RaycastHit hit, Quaternion.identity, _floorDistance * _heightModifier))
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
        if (_isRocketJumping) return;
        _rb.velocity = new Vector3(_input.move.x * _speed, _rb.velocity.y);
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

    private IEnumerator Shoot(int bulletType)
    {
        _canShoot = false;

        // Vector3 bulletDirection = (_aimPosition - _arm.position).normalized;
        // aca revisar _aimPosition porque creo que no es necesario tenerlo
        Vector3 bulletDirection = _arm.right;
        _shooting.Shoot(bulletType, _shootingPos.position, bulletDirection, _bulletSpeed);
        yield return new WaitForSeconds(_fireRate);

        _canShoot = true;
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

    #region RocketJumping
    public void RocketJumping(bool value)
    {
        _isRocketJumping = value;

        if (_isRocketJumping)
            _rocketTimer = _rocketJumpingTimer;
    }
    #endregion

    #region Melee
    private IEnumerator Melee()
    {
        // falta que use la direccion del mouse
        // o la rotacion del personaje
        _canMelee = false;

        // rota el "Bate"
        _meleeArm.gameObject.SetActive(true);
        _meleeArm.DORotate(_meleeFinalPos, _meleeSpeed);

        yield return new WaitForSeconds(_meleeSpeed);

        // lo devuelve a su posicion inicial
        _meleeArm.gameObject.SetActive(false);
        _meleeArm.rotation = _meleeInitialRot;

        _canMelee = true;
    }
    #endregion
    private void OnDestroy()
    {
        _input.OnControlChanged -= ControlChanged;
    }

    #region Getters/Setters
    public Rigidbody GetRB
    {
        get { return _rb; }
    }
    #endregion
}
