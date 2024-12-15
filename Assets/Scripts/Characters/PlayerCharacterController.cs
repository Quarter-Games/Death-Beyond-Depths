using System;
using UnityEngine;
using UnityEngine.InputSystem;

abstract public class PlayerCharacterController : Character
{
    [SerializeField] InputActionReference movementInputAction;
    [SerializeField] InputActionReference RunInputAction;
    [SerializeField] InputActionReference InteractInputAction;
    [SerializeField] protected InputActionReference CouchInputAction;

    #region Crouching
    [SerializeField, Range(0, 1)] float CrouchSpeedModifier;
    [SerializeField] CapsuleCollider2D Collider;
    public bool IsHidden { get; set; } = false;
    public bool CanCrouch { get; set; } = true;
    public bool IsStanding { get; private set; } = true;
    #endregion
    private const float MIN_FLOAT = 0.02f;

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
        Move(movement, RunInputAction.action.ReadValue<float>() > 0 && IsStanding ? MovementMode.Running : MovementMode.Walking);
        if(InteractInputAction.action.ReadValue<float>() != 0)
        {
            Interact();
        }
        if(CouchInputAction.action.triggered)
        {
            Debug.Log("Crouch");
        }
    }

    private void Interact()
    {
        InteractablesManager.Instance.Interact();
    }

    public void OnCrouchPerformed(InputAction.CallbackContext value)
    {
        if (!value.started || !CanCrouch)
        {
            return;
        }
        IsStanding = !IsStanding;
        ToggleCrouching();
        stats.MovementSpeed *= !IsStanding ? CrouchSpeedModifier : (1 / CrouchSpeedModifier);
    }

    private void ToggleCrouching()
    {
        Collider.direction = Collider.direction == CapsuleDirection2D.Vertical ? CapsuleDirection2D.Horizontal : CapsuleDirection2D.Vertical;
        animator.SetBool("IsCrouching", !IsStanding);
    }

    internal void Hide()
    {
        IsHidden = true;
        transform.position += Vector3.back * MIN_FLOAT;
    }

    internal void StopHiding()
    {
        IsHidden = false;
        transform.position += Vector3.forward * MIN_FLOAT;
    }

    abstract protected void SpecialMove();
}
