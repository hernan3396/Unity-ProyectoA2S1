using UnityEngine;
using System.Collections;

public class Player : Entity
{
    private enum States
    {
        Idle,
        Running,
        Jumping,
        Crouching,
        CrouchRunning,
        Falling,
        RocketJumping,
        Recoil,
        Melee,
        Dead
    }

    #region Components
    [Header("Components")]
    [SerializeField] private Animator _modelAnimator;
    private SavesManager _savesManager;
    private InventoryManager _invManager;
    private UIController _uiController;
    private Rigidbody _rb;
    private Inputs _input;
    private Camera _cam;
    #endregion

    #region Parameters
    [SerializeField] private PlayerData _playerData;
    private States _currentState;
    private float _fallingMaxSpeed;
    private int _gravityScale;
    private int _jumpForce;
    private int _jumpTime;
    #endregion

    #region CameraShake
    [Header("Camera Shake")]
    private CameraBehaviour _cameraBehaviour;
    private float _damageShake;
    private float _shakeTime;
    #endregion

    #region BodyParts
    [SerializeField] private Transform _particlePosOff;
    [SerializeField] private Transform _shootingPos;
    [SerializeField] private Transform _meleeArm;
    [SerializeField] private Transform _model;
    [SerializeField] private Transform _arm;
    [SerializeField] private GameObject _gunModel;
    [SerializeField] private GameObject _batModel;
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
    private bool _recoil;
    #endregion

    #region Aiming
    [Header("Aiming")]
    [SerializeField] private Transform _aimDebugSphere;
    private bool _canShoot = true;
    private bool _isMelee = false;
    private Vector3 _aimPosition;
    private bool _isMouse = true; // para ver que tipo de input estas usando
    #endregion

    #region Crouching
    [Header("Crouching")]
    [SerializeField] private GameObject _crouchingHitbox;
    [SerializeField] private GameObject _standingHitbox;
    private bool _crouching = false; // se podria hacer sin esta variable pero asi se ejecuta solo las veces necesarias el metodo Crouch()
    #endregion

    #region Pause
    private Vector2 _lastVelocity;
    private bool _onPause;
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
        _savesManager = GameManager.GetInstance.GetSavesManager;
        _cameraBehaviour = GameManager.GetInstance.GetCameraBehaviour;
        _uiController = GameManager.GetInstance.GetUIController;
        _invManager = GameManager.GetInstance.GetInvManager;
        _dustPool = GameManager.GetInstance.GetDustPool;
        _cam = GameManager.GetInstance.GetMainCamera;
        _input = GameManager.GetInstance.GetInput;

        SetLastCheckpointStats();

        _input.OnControlChanged += ControlChanged;
        GameManager.GetInstance.onGamePause += OnPause;
        GameManager.GetInstance.onGameOver += OnGameOver;
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
        _fallingMaxSpeed = _playerData.FallingMaxSpeed;

        _speed = _playerData.Speed;

        _damageShake = _playerData.DamageShake;
        _shakeTime = _playerData.ShakeTime;

        _weaponList = _playerData.WeaponList;
    }

    private void SetLastCheckpointStats()
    {
        _transform.position = _savesManager.GetCurrentCheckpoint;

        int lastHp = _savesManager.GetHealth;

        if (lastHp > 0)
            _currentHP = lastHp;

        _uiController.UpdateHealthPoints(_currentHP);
    }
    #endregion

    private void Update()
    {
        if (_currentState == States.Dead) return;
        if (_onPause) return;

        ManageState();

        if (_currentState != States.Melee)
            Aim();

        // la parte de disparar la hice por fuera de los estados
        // porque siempre podes disparar
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

        if (_currentState == States.Running || _currentState == States.Idle || _currentState == States.Crouching)
        {
            if (_input.jump) Jump();
            if (_input.Crouching) Crouch(true);
            if (_crouching && !_input.Crouching) Crouch(false);
            return;
        }

        if (_currentState == States.Jumping)
        {
            _jumpTimer -= Time.deltaTime;

            if (_jumpTimer <= 0 || !_input.jump)
                StopJump();

            return;
        }
    }

    private void FixedUpdate()
    {
        if (_currentState == States.Dead) return;
        if (_onPause) return;

        _rb.AddForce(Physics.gravity * _gravityScale, ForceMode.Acceleration); // simula una gravedad mas pesada

        if (_rb.velocity.y < -_fallingMaxSpeed)
            _rb.velocity = new Vector3(_rb.velocity.x, -_fallingMaxSpeed, _rb.velocity.z);

        if (_currentState != States.RocketJumping && _currentState != States.Recoil)
        {
            HorizontalMovement();
            return;
        }

        if (_currentState == States.RocketJumping)
        {
            _rb.AddForce(new Vector2(_input.move.x * _rocketImpulse, 0), ForceMode.Impulse);
            return;
        }
    }

    #region HorizontalMovement
    protected void HorizontalMovement()
    {
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
            _aimDebugSphere.position = _aimPosition;
            _arm.right = _aimPosition - _arm.position;
        }
        else
        {
            // control
            _aimDebugSphere.position = (Vector2)_transform.position + (_input.look * 5);
            _arm.right = _input.look;
        }

        if (_arm.right.x > 0)
            _model.forward = new Vector3(1, 0, 0);
        else
            _model.forward = new Vector3(-1, 0, 0);
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

        // cameraShake
        _cameraBehaviour.ShakeCamera(weaponData.ShootShake, weaponData.ShakeTime);

        // recoil de algunas armas
        StartCoroutine(Recoil(weaponData.RecoilForce, weaponData.RecoilTime));

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

        _cameraBehaviour.ShakeCamera(_damageShake, _shakeTime);
        _uiController.UpdateHealthPoints(_currentHP);
    }
    #endregion

    #region Death
    public void DebugDead()
    {
        Death();
    }

    protected override void Death()
    {
        ChangeState(States.Dead);
        GameManager.GetInstance.StartGameOver();
    }

    private void OnGameOver()
    {
        SetLastCheckpointStats();
        ChangeState(States.Idle);
    }
    #endregion

    #region Melee
    protected override IEnumerator Melee(WeaponData weaponData)
    {
        // ahora usamos la direccion del mouse
        // para hacer el ataque a melee
        // solo es aparecer la hitbox que rota
        // con el brazo

        // lo mas seguro que tengas que desactivar el
        // arma cuando este hecho eso

        _canShoot = false;
        _isMelee = !_canShoot;

        // aparece el brazo
        _meleeArm.gameObject.SetActive(true);
        _batModel.SetActive(true);
        _gunModel.SetActive(false);

        _cameraBehaviour.ShakeCamera(weaponData.ShootShake, weaponData.ShakeTime);

        yield return new WaitForSeconds(weaponData.FireRate);

        // "apaga" el brazo
        _meleeArm.gameObject.SetActive(false);
        _batModel.SetActive(false);
        _gunModel.SetActive(true);

        _canShoot = true;
        _isMelee = !_canShoot;
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

    // esta puesto aca porque de momento solo funciona para el rocketjump
    private IEnumerator Recoil(int RecoilForce, float recoilTime)
    {
        _recoil = true;

        _rb.AddForce(-_arm.right * RecoilForce, ForceMode.Impulse);

        yield return new WaitForSeconds(recoilTime);
        _recoil = false;
    }
    #endregion

    #region States
    private void ManageState()
    {
        if (_isMelee)
        {
            ChangeState(States.Melee);
            return;
        }

        if (_recoil)
        {
            ChangeState(States.Recoil);
            return;
        }

        if (_isRocketJumping)
        {
            ChangeState(States.RocketJumping);
            return;
        }

        // en vez de 0 le puse 
        // un dos para que en
        // el cambio de animaciones
        // no se chotee
        if (_rb.velocity.y > 0)
        {
            ChangeState(States.Jumping);
            return;
        }

        // a veces estas parado en el
        //  piso con una velocidad
        // muy chica (casi 0)
        if (_rb.velocity.y < 0 && !_isGrounded)
        {
            ChangeState(States.Falling);
            return;
        }

        if (_crouching && (Mathf.Abs(_rb.velocity.x) > 0 || _input.move.x != 0))
        {
            ChangeState(States.CrouchRunning);
            return;
        }

        if (_crouching)
        {
            ChangeState(States.Crouching);
            return;
        }

        if (Mathf.Abs(_rb.velocity.x) > 0 || _input.move.x != 0)
        {
            ChangeState(States.Running);
            return;
        }

        ChangeState(States.Idle);
    }

    private void ChangeState(States newState)
    {
        // https://docs.unity3d.com/ScriptReference/Animator.Play.html
        // ver ese link con normalizeTime o hacerlo a mano con los bool/trigger del animator
        // hacer el cambio de animacion
        _currentState = newState;

        string currentStateString = _currentState.ToString();

        if (_currentState == States.Melee)
            _modelAnimator.Play(currentStateString, 1);
        else
            _modelAnimator.Play(currentStateString);

        _uiController.UpdateState(currentStateString);
    }
    #endregion

    #region Pause
    private void OnPause(bool value)
    {
        _onPause = value;

        if (_onPause)
            PausePlayer();
        else
            ResumePlayer();
    }

    private void PausePlayer()
    {
        _lastVelocity = _rb.velocity;
        _rb.velocity = Vector2.zero;
        _rb.useGravity = false;
        _modelAnimator.speed = 0;
    }

    private void ResumePlayer()
    {
        _rb.velocity = _lastVelocity;
        _rb.useGravity = true;
        _modelAnimator.speed = 1;
    }
    #endregion

    private void OnDestroy()
    {
        _input.OnControlChanged -= ControlChanged;
        GameManager.GetInstance.onGamePause -= OnPause;
        GameManager.GetInstance.onGameOver -= OnGameOver;
    }

    #region Getter/Setter
    public Rigidbody GetRB
    {
        get { return _rb; }
    }

    public int GetHealth
    {
        get { return _currentHP; }
    }
    #endregion
}
