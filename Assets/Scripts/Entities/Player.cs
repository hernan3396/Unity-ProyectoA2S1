using UnityEngine;
using System.Collections;

public class Player : Entity
{
    #region Components
    private InventoryManager _invManager;
    private UIController _uiController;
    private Rigidbody _rb;
    private Inputs _input;
    private Camera _cam;
    #endregion

    #region Parameters
    [SerializeField] private PlayerData _playerData;
    private int _gravityScale;
    private int _jumpForce;
    private int _jumpTime;
    #endregion

    #region BodyParts
    [SerializeField] private Transform _particlePosOff;
    [SerializeField] private Transform _shootingPos;
    [SerializeField] private Transform _meleeArm;
    [SerializeField] private Transform _arm;
    #endregion

    #region Jumping
    private bool _isGrounded = true;
    private bool _jumping = false;
    private PoolManager _dustPool;
    private float _jumpTimer;
    #endregion

    #region RocketJumping
    [Header("Rocket Jumping")]
    [SerializeField] private int _rocketImpulse = 5;
    private bool _isRocketJumping;
    #endregion

    #region Aiming
    private bool _canShoot = true;
    private Vector3 _aimPosition;
    private bool _isMouse = true; // para ver que tipo de input estas usando
    #endregion

    #region Crouching
    [Header("Crouching")]
    [SerializeField] private GameObject _crouchingHitbox;
    [SerializeField] private GameObject _standingHitbox;
    private bool _crouching = false; // se podria hacer sin esta variable pero asi se ejecuta solo las veces necesarias el metodo Crouch()
    #endregion

    protected override void Awake()
    {
        base.Awake();
        SetStats();

        _rb = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        base.Start();
        _uiController = GameManager.GetInstance.GetUIController;
        _invManager = GameManager.GetInstance.GetInvManager;
        _dustPool = GameManager.GetInstance.GetDustPool;
        _cam = GameManager.GetInstance.GetMainCamera;
        _input = GameManager.GetInstance.GetInput;

        _uiController.UpdateHealthPoints(_currentHP);

        _input.OnControlChanged += ControlChanged;
    }

    #region Parameters
    private void SetStats()
    {
        _hp = _playerData.Hp;
        _currentHP = _hp;
        _invulnerability = _playerData.Invulnerability;

        _gravityScale = _playerData.GravityScale;
        _jumpForce = _playerData.JumpForce;
        _jumpTime = _playerData.JumpTime;

        _speed = _playerData.Speed;

        _weaponList = _playerData.WeaponList;
    }
    #endregion

    private void Update()
    {
        Aim();

        if (_isGrounded && _input.jump)
            Jump();

        if (_isGrounded && _input.Crouching)
            Crouch(true);

        if (_crouching && !_input.Crouching)
            Crouch(false);

        if (_jumping)
        {
            _jumpTimer -= Time.deltaTime;

            if (_jumpTimer <= 0 || !_input.jump)
                StopJump();
        }

        if (_canShoot)
        {
            if (_input.IsShooting)
            {
                StartCoroutine(Shoot(_weaponList[(int)WeaponData.Weapons.TwinPistols]));
                return;
            }

            if (_input.CannonShooting)
            {
                StartCoroutine(Shoot(_weaponList[(int)WeaponData.Weapons.RocketLauncher]));
                return;
            }

            if (_input.Melee)
            {
                StartCoroutine(Melee(_weaponList[(int)WeaponData.Weapons.Bat]));
            }
        }
    }

    private void FixedUpdate()
    {
        SetNextWaypoint();

        _rb.AddForce(Physics.gravity * _gravityScale, ForceMode.Acceleration); // simula una gravedad mas pesada
    }

    #region HorizontalMovement
    protected override void SetNextWaypoint()
    {
        if (_isRocketJumping)
        {
            _rb.AddForce(new Vector2(_input.move.x * _rocketImpulse, 0), ForceMode.Impulse);
            return;
        }

        _rb.velocity = new Vector3(_input.move.x * _speed, _rb.velocity.y);
    }
    #endregion

    #region Grounded
    // para no andar chambiando con _isGrounded = !_isGrounded
    // porque asumo que puede llegar a dar algun problema
    public void IsGrounded()
    {
        _isGrounded = true;
        _isRocketJumping = false;
    }

    public void NotGrounded()
    {
        _isGrounded = false;
    }
    #endregion

    #region Jumping
    private void Jump()
    {
        _jumping = true;
        _jumpTimer = _jumpTime; // timer para limitar el salto
        _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _rb.velocity.z);

        GameObject dust = _dustPool.GetPooledObject();
        if (!dust) return;

        // sino aparece en el centro
        Vector3 dustPosition = _particlePosOff.position;
        dust.transform.position = dustPosition;
        dust.SetActive(true);
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
    protected override void Aim()
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
            // control
            _arm.right = _input.look;
        }
    }

    protected override IEnumerator Shoot(WeaponData weaponData)
    {
        // chequea si hay balas
        if (_invManager.GetAmount((int)weaponData.BulletType) <= 0) yield break;

        // si hay balas sigue adelante
        _canShoot = false;

        Vector3 bulletDirection = _arm.right;
        _shooting.Shoot((int)weaponData.BulletType, _shootingPos.position, bulletDirection, weaponData.BulletSpeed);

        // consume una bala del inventario
        _invManager.RemoveAmount((int)weaponData.BulletType, 1);

        yield return new WaitForSeconds(weaponData.FireRate);

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
    public void AddHealth(int value)
    {
        if (_currentHP + value <= _hp)
        {
            _currentHP += value;
        }
        else
        {
            _currentHP = _hp;
        }

        _uiController.UpdateHealthPoints(_currentHP);
    }

    public override void TakeDamage(int value)
    {
        base.TakeDamage(value);
        _uiController.UpdateHealthPoints(_currentHP);
    }

    protected override void Death()
    {
        GameManager.GetInstance.GameOver();
    }
    #endregion

    #region Melee
    protected override IEnumerator Melee(WeaponData weaponData)
    {
        // ahora usamos la direccion del mouse
        // para hacer el ataque a melee
        // solo es aparecer la hitbox que rota
        // con el brazo
        _canShoot = false;

        // aparece el brazo
        _meleeArm.gameObject.SetActive(true);

        yield return new WaitForSeconds(weaponData.FireRate);

        // "apaga" el brazo
        _meleeArm.gameObject.SetActive(false);

        _canShoot = true;
    }
    #endregion

    #region Crouching
    private void Crouch(bool value)
    {
        _crouchingHitbox.SetActive(value);
        _standingHitbox.SetActive(!value);
        _crouching = value;
    }
    #endregion

    #region RocketJumping
    public void RocketJumping(bool value)
    {
        _isRocketJumping = value;
    }
    #endregion

    private void OnDestroy()
    {
        _input.OnControlChanged -= ControlChanged;
    }

    #region Getter/Setter
    public Rigidbody GetRB
    {
        get { return _rb; }
    }
    #endregion
}
