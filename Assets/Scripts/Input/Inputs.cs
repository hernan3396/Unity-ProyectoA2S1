using UnityEngine;
using UnityEngine.InputSystem;

public class Inputs : MonoBehaviour
{
    // este script lee las inputs y vos
    // luego las lees de otro y haces las cosas

    #region CharacterInputValues
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool IsShooting;
    private bool _canMove = true;
    public bool CannonShooting;
    private bool _cursorInputForLook = true;
    #endregion

    #region OnControlChange
    public delegate void ControlChanged(string control);
    public ControlChanged OnControlChanged;
    #endregion

    public void OnPause()
    {
        GameManager.GetInstance.PauseGame();
    }

    // aca esta bien, puedo hacer una referencia al gamemanager para la pausa (por ej)
    // o pasarlo al gamemanager y leer de ahi
    public void OnJump(InputValue value)
    {
        if (!_canMove) return;
        JumpInput(value.isPressed);
    }

    public void OnMove(InputValue value)
    {
        if (!_canMove) return;
        MoveInput(value.Get<Vector2>());
    }

    public void OnAim(InputValue value)
    {
        // chequear cual es el input
        if (_cursorInputForLook && _canMove)
            LookInput(value.Get<Vector2>());
    }

    public void OnShoot(InputValue value)
    {
        ShootInput(value.isPressed);
    }

    public void OnCannonShoot(InputValue value)
    {
        CannonShootInput(value.isPressed);
    }

    private void OnControlsChanged(PlayerInput action)
    {
        if (OnControlChanged != null)
        {
            string device = action.devices[0].ToString();
            string splitDevice = device.Split(":/")[0]; // divide el nombre del dispositivo en algo mas legible
                                                        // Debug.Log(splitDevice);
            OnControlChanged.Invoke(splitDevice);
        }
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        // Debug.Log(newLookDirection);
        look = newLookDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }

    public void ShootInput(bool newShootState)
    {
        IsShooting = newShootState;
    }

    public void CannonShootInput(bool newShootState)
    {
        CannonShooting = newShootState;
    }
}