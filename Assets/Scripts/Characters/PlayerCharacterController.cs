using DG.Tweening;
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
    [SerializeField] InputActionReference BackStepInputAction;
    [SerializeField] protected InputActionReference CrouchInputAction;
    [SerializeField] PlayerMeleeAttack MeleeAttackObject;
    [SerializeField] private InputActionMap InputActionMap;
    public static bool IsRaightClickHold = false;

    [Header("Climbing")]
    [SerializeField] float ClimbingSpeed = 1f;

    [Header("Backstep")]
    [SerializeField] float BackStepDurationInSeconds = 0.75f;
    [SerializeField] float BackStepCooldownInSeconds = 1f;
    [SerializeField] float BackStepSpeed = 3f;
    private Coroutine BackStepCoroutine;

    bool IsFacingRight = true;

    public bool IsMeleeAttacking = false;
    public bool IsRangeAttacking = false;
    private bool backStepAnimationComplete = false;

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
        CrouchInputAction.action.performed += OnCrouchPerformed;
        LeftClickInputAction.action.performed += LeftMouseClick;
        RightClickInputAction.action.performed += RightMouseHold;
        InteractInputAction.action.started += Interact;
        BackStepInputAction.action.started += BackStep;
    }

    protected override void OnDisable()
    {
        CrouchInputAction.action.performed -= OnCrouchPerformed;
        LeftClickInputAction.action.performed -= LeftMouseClick;
        RightClickInputAction.action.performed -= RightMouseHold;
        InteractInputAction.action.started -= Interact;
        BackStepInputAction.action.started -= BackStep;
    }

    private void FixedUpdate()
    {
        Vector2 movementInput = movementInputAction.action.ReadValue<Vector2>();
        var movement = new Vector2();

        if (movementInput.x > 0)
        {
            movement = Vector2.right;
            IsFacingRight = true;
            Flip();
        }
        else if (movementInput.x < 0)
        {
            movement = Vector2.left;
            IsFacingRight = false;
            Flip();
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

    protected override void Flip()
    {
        transform.localScale = new Vector3(
            IsFacingRight ? -Mathf.Abs(transform.localScale.x) : Mathf.Abs(transform.localScale.x),
            transform.localScale.y,
            transform.localScale.z
        );
    }

    private void BackStep(InputAction.CallbackContext context)
    {
        if (BackStepCoroutine != null || !IsStanding) return;
        rb.linearVelocityX = 0f;
        Vector2 direction = IsFacingRight ? Vector2.left : Vector2.right;
        rb.AddForce(BackStepSpeed * direction, ForceMode2D.Impulse);
        StartCoroutine(BackStepAction());
    }
    private IEnumerator BackStepAction()
    {
        backStepAnimationComplete = false;
        DisableInput();
        BackStepCoroutine = StartCoroutine(stats.BecomeInvincibleForSeconds(BackStepDurationInSeconds));
        StartCoroutine(BackStepCooldown());
        animator.SetTrigger("Backstepping");
        yield return new WaitUntil(() => backStepAnimationComplete);
        EnableInput();
    }

    //animation callback
    private void OnBackStepAnimationComplete()
    {
        backStepAnimationComplete = true;
    }

    private IEnumerator BackStepCooldown()
    {
        yield return new WaitForSeconds(BackStepCooldownInSeconds);
        BackStepCoroutine = null;
    }

    private static void DisableInput()
    {
        foreach (var action in InputSystem.actions)
        {
            action.Disable();
        }
    }
    private static void EnableInput()
    {
        foreach (var action in InputSystem.actions)
        {
            action.Enable();
        }
    }

    public abstract void RightMouseHold(InputAction.CallbackContext context);

    abstract protected void SpecialMove();
    public void StartClimbing(ClimablePoint startPoint)
    {
        rb.simulated = false;
        Collider.enabled = false;
        float time = Vector3.Distance(startPoint.SnippingPoint.position, startPoint.LinkedPoint.SnippingPoint.position) / ClimbingSpeed;
        DisableInput();
        transform.DOMove(startPoint.LinkedPoint.SnippingPoint.position, time).SetEase(Ease.Linear).OnComplete(() => StopClimbing());
    }
    public void StopClimbing()
    {
        rb.simulated = true;
        Collider.enabled = true;
        EnableInput();
    }
}
