using UnityEngine;
using UnityEngine.InputSystem;

abstract public class PlayerCharacterController : Character
{
    [SerializeField] InputActionReference movementInputAction;
    [SerializeField] InputActionReference RunInputAction;
    [SerializeField] protected InputActionReference specialMoveInputAction;

    private void FixedUpdate()
    {
        Vector2 movementInput = movementInputAction.action.ReadValue<Vector2>();

        var movement = new Vector2();
        if (movementInput.x > 0)
        {
            movement = Vector2.right;
        }
        else if (movementInput.x < 0)
        {
            movement = Vector2.left;
        }
        Move(movement, RunInputAction.action.ReadValue<float>() > 0 ? MovementMode.Running : MovementMode.Walking);
    }

    public void OnSpecialMovePerformed(InputAction.CallbackContext value)
    {
        if (!value.started)
        {
            return;
        }
        SpecialMove();
    }

    abstract protected void SpecialMove();
}
