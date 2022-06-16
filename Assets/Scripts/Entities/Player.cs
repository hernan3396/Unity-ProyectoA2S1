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

    private enum SFX
    {
        Jump,
        Damage
    }

    #region Components
    [Header("Components")]
    [SerializeField] private Animator _modelAnimator;
    private SavesManager _savesManager;
    private InventoryManager _invManager;
    private UIController _uiController;
    private AudioManager _audioManager;
    private Rigidbody _rb;
    private Inputs _input;
    private Camera _cam;
    #endregion

    #region Parameters
    [SerializeField] private PlayerData _playerData;
    private States _currentState = States.Idle;
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
        _audioManager = AudioManager.GetInstance;

        SetLastCheckpointStats();

        _input.OnControlChanged += ControlChanged;
        GameManager.GetInstance.onGamePause += OnPause;
        GameManager.GetInstance.onGameOver += OnGameOver;

        _uiController.UpdateState(_currentState.ToString());
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

        switch (_currentState)
        {
            case States.Idle:
                Idle();
                break;

            case States.Running:
                Running();
                break;

            case States.Crouching:
                Crouching();
                break;

            case States.CrouchRunning:
                CrouchRunning();
                break;

            case States.Jumping:
                Jumping();
                break;

            case States.Falling:
                Falling();
                break;

            case States.RocketJumping:
                RocketJumping();
                break;

            case States.Recoil:
                Recoil();
                break;

            case States.Melee:
                MeleeState();
                break;
        }
    }

    private void FixedUpdate()
    {
        if (_currentState == States.Dead) return;
        if (_onPause) return;

        _rb.AddForce(Physics.gravity * _gravityScale, ForceMode.Acceleration); // simula una gravedad mas pesada

        // limitar la velocidad de caida
        if (_rb.velocity.y < -_fallingMaxSpeed)
            _rb.velocity = new Vector3(_rb.velocity.x, -_fallingMaxSpeed, _rb.velocity.z);
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
        if (!_isGrounded) return;

        _jumping = true;
        _jumpTimer = _jumpTime; // timer para limitar el salto
        _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _rb.velocity.z);

        _audioManager.PlaySound(AudioManager.AudioList.PlayerSFX);
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

        // Reproduce un sonido
        _audioManager.PlaySound(AudioManager.AudioList.Gunshoots, false, 0);

        // consume una bala del inventario
        _invManager.RemoveAmount((int)weaponData.BulletType, 1);

        // cameraShake
        _cameraBehaviour.ShakeCamera(weaponData.ShootShake, weaponData.ShakeTime);

        // recoil de algunas armas
        if (weaponData.RecoilTime > 0)
        {
            StartCoroutine(Recoil(weaponData.RecoilForce, weaponData.RecoilTime));
            ChangeState(States.Recoil);
        }

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

        _audioManager.PlaySound(AudioManager.AudioList.PlayerSFX, false, (int)SFX.Damage);
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
        _canShoot = false;
        _isMelee = !_canShoot;

        // aparece el brazo
        _meleeArm.gameObject.SetActive(true);
        _batModel.SetActive(true);
        _gunModel.SetActive(false);

        _cameraBehaviour.ShakeCamera(weaponData.ShootShake, weaponData.ShakeTime);
        ChangeState(States.Melee);

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
        ChangeState(States.RocketJumping);
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
    private void Idle()
    {
        Aim();
        HorizontalMovement(); // esta para frenar al player en 0 cuando estas moviendote
        _modelAnimator.SetBool("isRunning", false);
        _modelAnimator.SetBool("isCrouching", false);
        _modelAnimator.SetBool("isFalling", false);
        _modelAnimator.SetBool("isRocketJumping", false);
        _modelAnimator.SetBool("isRecoil", false);

        if (_input.jump)
            ChangeState(States.Jumping);

        if (_input.move.x != 0)
            ChangeState(States.Running);

        if (_input.Crouching)
        {
            Crouch(true);
            ChangeState(States.Crouching);
        }
    }

    private void Running()
    {
        Aim();
        HorizontalMovement();
        _modelAnimator.SetBool("isRunning", true);

        if (_rb.velocity.x == 0 && _input.move.x == 0)
            ChangeState(States.Idle);

        if (_input.Crouching)
        {
            Crouch(true);
            ChangeState(States.Crouching);
        }

        if (_input.jump)
            ChangeState(States.Jumping);

        if (!_isGrounded)
            ChangeState(States.Falling);
    }

    private void Crouching()
    {
        Aim();
        _modelAnimator.SetBool("isCrouching", true);
        _modelAnimator.SetBool("isRunning", false);

        if (!_input.Crouching)
        {
            Crouch(false);
            ChangeState(States.Idle);
        }

        if (_input.move.x != 0)
            ChangeState(States.CrouchRunning);
    }

    private void CrouchRunning()
    {
        HorizontalMovement();
        Aim();
        _modelAnimator.SetBool("isRunning", true);

        if (_rb.velocity.x == 0 && _input.move.x == 0)
            ChangeState(States.Crouching);

        if (!_input.Crouching)
        {
            Crouch(false);
            ChangeState(States.Idle);
        }

        if (!_isGrounded)
            ChangeState(States.Falling);
    }

    private void Jumping()
    {
        Aim();
        Jump();
        HorizontalMovement();
        _modelAnimator.SetTrigger("Jumping");

        if (_rb.velocity.y < 0)
            ChangeState(States.Falling);
    }

    private void Falling()
    {
        Aim();
        HorizontalMovement();
        _modelAnimator.ResetTrigger("Jumping");
        _modelAnimator.SetBool("isFalling", true);

        if (_isGrounded)
            ChangeState(States.Idle);
    }

    private void RocketJumping()
    {
        Aim();
        _rb.AddForce(new Vector2(_input.move.x * _rocketImpulse, 0), ForceMode.Impulse);
        _modelAnimator.SetBool("isRocketJumping", true);

        if (_isGrounded)
            ChangeState(States.Idle);
    }

    private void Recoil()
    {
        _modelAnimator.SetBool("isRecoil", true);

        if (!_recoil)
            ChangeState(States.Idle);
    }

    private void MeleeState()
    {
        HorizontalMovement();
        _modelAnimator.Play(_currentState.ToString(), 1);

        if (!_isMelee)
            ChangeState(States.Idle);
    }

    private void ChangeState(States newState)
    {
        // https://docs.unity3d.com/ScriptReference/Animator.Play.html
        // ver ese link con normalizeTime o hacerlo a mano con los bool/trigger del animator
        // hacer el cambio de animacion
        _currentState = newState;

        string currentStateString = _currentState.ToString();

        // if (_currentState == States.Melee)
        //     _modelAnimator.Play(currentStateString, 1);
        // else
        //     _modelAnimator.Play(currentStateString);

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
