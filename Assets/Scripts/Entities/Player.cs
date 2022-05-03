using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class Player : MonoBehaviour
{

    [SerializeField] private int _speed = 10;
    [SerializeField] private int _jumpForce = 15;
    private bool _isGrounded = false;
    private PlayerInput _playerInput;
    private Rigidbody _rb;
    private Inputs _input;

    private void Start()
    {
        _input = GetComponent<Inputs>();
        _rb = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        Move();
        if (!_isGrounded) { return; }
        // Look();
        Jump();
    }

    private void Move()
    {
        // Debug.Log(_input.move);
        _rb.velocity = new Vector3(_input.move.x * _speed, _rb.velocity.y);
    }

    private void Jump()
    {
        if (_input.jump)
        {
            _rb.velocity = Vector3.up * _jumpForce;
            // _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _input.jump = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            _isGrounded = false;
        }
    }
}
