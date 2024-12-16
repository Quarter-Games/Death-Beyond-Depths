using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

abstract public class PlayerCharacterController : Character
{
    [SerializeField] InputActionReference movementInputAction;
    [SerializeField] InputActionReference RunInputAction;
    [SerializeField] InputActionReference InteractInputAction;
    [SerializeField] InputActionReference LeftClickInputAction;
    [SerializeField] InputActionReference RightClickInputAction;
    [SerializeField] protected InputActionReference CouchInputAction;
    [SerializeField] PlayerMeleeAttack MeleeAttackObject;
    public static bool IsRaightClickHold = false;

    public bool IsMeleeAttacking = false;
    public bool IsRangeAttacking = false; // TODO probably need to move this to captain's class

    #region Crouching
    [SerializeField, Range(0, 1)] float CrouchSpeedModifier;
    [SerializeField] CapsuleCollider2D Collider;
    public bool IsHidden { get; set; } = false;
    public bool CanCrouch { get; set; } = true;
    public bool IsStanding { get; private set; } = true;
    #endregion
    private const float MIN_FLOAT = 0.02f;

    protected override void OnEnable()
    {
        CouchInputAction.action.performed += OnCrouchPerformed;
        LeftClickInputAction.action.performed += LeftMouseClick;
        RightClickInputAction.action.performed += RightMouseHold;
        InteractInputAction.action.started += Interact;

    }

    protected override void OnDisable()
    {
        CouchInputAction.action.performed -= OnCrouchPerformed;
        LeftClickInputAction.action.performed -= LeftMouseClick;
        RightClickInputAction.action.performed -= RightMouseHold;
        InteractInputAction.action.started -= Interact;
    }

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

    }

    private void OnMeleeAttackPerformed(InputAction.CallbackContext context)
    {
        if (IsRangeAttacking || IsMeleeAttacking)
            return;
        StartCoroutine(WaitForCoolDown(MeleeAttackObject.Stats.CooldownTime, true));
        IsMeleeAttacking = true;
        MeleeAttackObject.gameObject.SetActive(true);
    }
    protected IEnumerator WaitForCoolDown(float time, bool isMelee)
    {
        var startTimeTime = Time.time;
        yield return new WaitUntil(() => Time.time - startTimeTime > time);
        if (isMelee) IsMeleeAttacking = false;
        else IsRangeAttacking = false;
    }
    private void Interact(InputAction.CallbackContext value)
    {
        if (!value.started)
        {
            return;
        }
        InteractablesManager.Instance.Interact();
    }

    public void OnCrouchPerformed(InputAction.CallbackContext value)
    {
        if (!value.performed || !CanCrouch)
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
    public virtual void LeftMouseClick(InputAction.CallbackContext context)
    {
        OnMeleeAttackPerformed(context);
    }
    public abstract void RightMouseHold(InputAction.CallbackContext context);

    abstract protected void SpecialMove();
}
