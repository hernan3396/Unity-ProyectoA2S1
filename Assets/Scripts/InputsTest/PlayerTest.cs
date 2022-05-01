using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] private int _speed = 10;
    private bool _isGrounded = true;
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
        Look();
        Jump();
    }

    private void Move()
    {
        // Debug.Log(_input.move);
        _rb.velocity = new Vector3(_input.move.x * _speed, _rb.velocity.y);
    }

    private void Jump()
    {
        // Debug.Log(_input.jump);
    }

    private void Look()
    {
        // Debug.Log(_input.look);
    }
}