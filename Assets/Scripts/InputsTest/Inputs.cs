using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
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
        private bool _cursorInputForLook = true;
        #endregion

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
    }
}