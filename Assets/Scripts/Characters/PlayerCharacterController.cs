using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static DamageVignetteController;

abstract public class PlayerCharacterController : Character
{
    [SerializeField] float AttackInterval = 1f;
    [SerializeField] InputActionReference movementInputAction;
    [SerializeField] InputActionReference RunInputAction;
    [SerializeField] InputActionReference InteractInputAction;
    [SerializeField] InputActionReference LeftClickInputAction;
    [SerializeField] InputActionReference RightClickInputAction;
    [SerializeField] InputActionReference BackStepInputAction;
    [SerializeField] InputActionReference EquipSwordAction;
    [SerializeField] protected InputActionReference CrouchInputAction;
    [SerializeField] PlayerMeleeAttack MeleeAttackObject;
    [SerializeField] ParticleSystem SlashEffect;
    [SerializeField] SortingGroup SortingGroup;
    [SerializeField] public PlayerStamina Stamina;
    public static bool IsRaightClickHold = false;

    [Header("Climbing")]
    [SerializeField] float ClimbingSpeed = 1f;

    [Header("Backstep")]
    [SerializeField] float BackStepDurationInSeconds = 0.75f;
    [SerializeField] float BackStepCooldownInSeconds = 1f;
    [SerializeField] float BackStepSpeed = 3f;
    [Header("IK")]
    [SerializeField] Transform LeftLegTarget;
    [SerializeField] Transform RightLegTarget;
    [SerializeField] float IKDisplacementMinHeight = 0.1f;
    [Header("Audio")]
    [SerializeField] SoundData WalkSound;
    [SerializeField] SoundData CrouchWalkSound;

    private Coroutine BackStepCoroutine;
    private float AttackIntervalTimer = 0;
    private int NumberOfEnemiesAwareOfPlayer;
    bool IsFacingRight = true;
    bool isSwordEquipped = false;
    public bool IsMeleeAttacking = false;
    public bool IsRangeAttacking = false;
    private bool backStepAnimationComplete = false;

    public static event Action<bool> OnFlip;

    #region Crouching
    [SerializeField, Range(0, 1)] float CrouchSpeedModifier;
    [SerializeField] Collider2D Collider;
    public bool IsHidden { get; set; } = false;
    public bool CanCrouch { get; set; } = true;
    public bool IsStanding { get; private set; } = true;
    #endregion
    public bool IsFacingLeft { get => !IsFacingRight; }
    private const float MIN_FLOAT = 0.02f;
    private const string CLIMB_ANIMATION = "Climb";
    private const string INTERACT_ANIMATION = "Interact";
    private const string DONE_CLIMBING_ANIMATION = "Done Climbing";

    private void OnValidate()
    {
        TryGetComponent(out Stamina);
    }

    protected override void OnEnable()
    {
        AttackIntervalTimer = AttackInterval;
        CrouchInputAction.action.performed += OnCrouchPerformed;
        LeftClickInputAction.action.performed += LeftMouseClick;
        RightClickInputAction.action.performed += RightMouseHold;
        InteractInputAction.action.started += Interact;
        //BackStepInputAction.action.started += BackStep;
        //EquipSwordAction.action.started += OnSwordEquip;
        IsFacingRight = transform.localScale.x < 0;
    }

    protected override void OnDisable()
    {
        CrouchInputAction.action.performed -= OnCrouchPerformed;
        LeftClickInputAction.action.performed -= LeftMouseClick;
        RightClickInputAction.action.performed -= RightMouseHold;
        InteractInputAction.action.started -= Interact;
        //BackStepInputAction.action.started -= BackStep;
        // EquipSwordAction.action.started -= OnSwordEquip;
    }

    protected void SetRotationTo(Vector2 direction)
    {
        if (direction.x > 0)
        {
            IsFacingRight = true;
            Flip();
        }
        else if (direction.x < 0)
        {
            IsFacingRight = false;
            Flip();
        }
    }
    private void FixedUpdate()
    {
        Vector2 movementInput = movementInputAction.action.ReadValue<Vector2>();
        var movement = new Vector2();

        if (movementInput.x > 0)
        {
            movement = Vector2.right;
            if (!IsRaightClickHold)
            {
                IsFacingRight = true;
                Flip();
            }
        }
        else if (movementInput.x < 0)
        {
            movement = Vector2.left;
            if (!IsRaightClickHold)
            {
                IsFacingRight = false;
                Flip();
            }
        }
        if (movementInput.x != 0)
        {
            SoundDataManager.Instance.EmitSound(transform, IsStanding ? WalkSound.SoundTravelDistance : CrouchWalkSound.SoundTravelDistance);
        }
        //FootPlacement();
        Move(movement, RunInputAction.action.ReadValue<float>() > 0 && IsStanding ? MovementMode.Running : MovementMode.Walking);
    }

    private void Update()
    {
        AttackIntervalTimer += Time.deltaTime;
    }

    public void FootPlacement()
    {
        float leftLegHeight = LeftLegTarget.position.y;
        float rightLegHeight = RightLegTarget.position.y;
        float minHeight = Mathf.Min(leftLegHeight, rightLegHeight);

        Bounds bounds = Collider.bounds;
        float currentWorldFloor = transform.position.y - bounds.extents.y + Collider.offset.y;
        float displacement = currentWorldFloor - minHeight;
        Collider.offset = new Vector2(Collider.offset.x, Collider.offset.y - displacement - IKDisplacementMinHeight);

    }

    private void OnMeleeAttackPerformed(InputAction.CallbackContext context)
    {
        if (!isSwordEquipped) return;
        if (IsRangeAttacking || IsMeleeAttacking || !IsStanding)
            return;
        if (AttackIntervalTimer < AttackInterval) return;
        Stamina.ConsumeStamina(4);
        DisableInput();
        InputSystem.actions.Where(a => a == movementInputAction.action).First().Enable();
        //SlashEffect.Play();
        animator.SetTrigger("Strike Sword");
        animator.SetFloat("Attack Chance", UnityEngine.Random.Range(0, 1f));
        IsMeleeAttacking = true;
        //StartCoroutine(WaitForCoolDown(MeleeAttackObject.Stats.CooldownTime, true));
        //MeleeAttackObject.gameObject.SetActive(true);
    }

    public void StartAttackCooldown()
    {
        EnableInput();
        AttackIntervalTimer = 0;
        animator.ResetTrigger("Strike Sword");
        IsMeleeAttacking = false;
        MeleeAttackObject.ResetEnemyAttackedList();
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
        if (InteractablesManager.Instance.IsInteractingWithClimable())
        {
            return;
        }
        animator.SetTrigger(INTERACT_ANIMATION);
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
    public void TweenCrouchValue()
    {
        float endValue = !IsStanding ? 2 : 0;
        float crouch = animator.GetFloat("Crouch");
        DOTween.To(() => animator.GetFloat("Crouch"), x => animator.SetFloat("Crouch", x), endValue, 0.1f);
    }
    private void ToggleCrouching()
    {
        //Collider.direction = Collider.direction == CapsuleDirection2D.Vertical ? CapsuleDirection2D.Horizontal : CapsuleDirection2D.Vertical;
        TweenCrouchValue();

    }

    internal void Hide()
    {
        if (IsHidden) return;
        IsHidden = true;
        SortingGroup.sortingOrder += 1;
    }

    internal void StopHiding()
    {
        if (!IsHidden) return;
        IsHidden = false;
        SortingGroup.sortingOrder -= 1;
    }
    public virtual void LeftMouseClick(InputAction.CallbackContext context)
    {
        OnMeleeAttackPerformed(context);
    }

    protected override void Flip()
    {
        float tempXScale = transform.localScale.x;
        transform.localScale = new Vector3(
            IsFacingRight ? -Mathf.Abs(transform.localScale.x) : Mathf.Abs(transform.localScale.x),
            transform.localScale.y,
            transform.localScale.z
        );
        if (tempXScale != transform.localScale.x)
        {
            OnFlip?.Invoke(IsFacingRight);
        }
    }

    private void BackStep(InputAction.CallbackContext context)
    {
        if (BackStepCoroutine != null || !IsStanding) return;
        StartCoroutine(BackStepAction());
    }

    private void OnSwordEquip(InputAction.CallbackContext context)
    {
        animator.SetTrigger("Enable Sword");
        isSwordEquipped = !animator.GetBool("IsSwordEquipped");
        animator.SetBool("IsSwordEquipped", isSwordEquipped);
    }

    private IEnumerator BackStepAction()
    {
        backStepAnimationComplete = false;
        DisableInput();
        BackStepCoroutine = StartCoroutine(stats.BecomeInvincibleForSeconds(BackStepDurationInSeconds));
        StartCoroutine(BackStepCooldown());
        animator.SetTrigger("Backstep");
        yield return new WaitUntil(() => backStepAnimationComplete);
        EnableInput();
    }

    public void BackStepForce()
    {
        rb.linearVelocityX = 0f;
        Vector2 direction = IsFacingRight ? Vector2.left : Vector2.right;
        rb.AddForce(BackStepSpeed * direction, ForceMode2D.Impulse);
    }

    //animation callback
    public void OnBackStepAnimationComplete()
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

    public static IEnumerator DisableThenEnableInputSeconds(float seconds)
    {
        DisableInput();
        yield return new WaitForSeconds(seconds);
        EnableInput();
    }

    public abstract void RightMouseHold(InputAction.CallbackContext context);

    abstract protected void SpecialMove();
    public void StartClimbing(ClimablePoint startPoint)
    {
        rb.simulated = false;
        Collider.enabled = false;
        float time = Vector2.Distance(startPoint.SnippingPoint.position, startPoint.LinkedPoint.SnippingPoint.position) / ClimbingSpeed;
        DisableInput();
        animator.SetBool(DONE_CLIMBING_ANIMATION, false);
        animator.SetTrigger(CLIMB_ANIMATION);
        var pos = startPoint.LinkedPoint.SnippingPoint.position;
        pos = new Vector3(pos.x, pos.y, transform.position.z);
        transform.DOMove(pos, time).SetEase(Ease.Linear).OnComplete(() => StopClimbing());
    }
    public void StopClimbing()
    {
        rb.simulated = true;
        Collider.enabled = true;
        EnableInput();
        animator.SetBool(DONE_CLIMBING_ANIMATION, true);
    }

    public void TakeDamage(int damage)
    {
        if (IsAttacked) return;
        IsAttacked = true;
        stats.TakeDamage(damage);
        animator.SetTrigger("Get hit");
        animator.SetInteger("Hit number", UnityEngine.Random.Range(0, 3));
        DisableInput();
        IsMeleeAttacking = false;
        MeleeAttackObject.ResetEnemyAttackedList();
        float knockbackDirection = IsFacingRight ? -1 : 1;
        rb.AddForce(new Vector2(5 * knockbackDirection, 3), ForceMode2D.Impulse);
        SpecialEffects.ScreenDamageEffect(UnityEngine.Random.Range(0.1f, 1));
        CameraController.Instance.ShakeCamera();
    }

    public void OnStoppedGettingHit()
    {
        EnableInput();
        IsAttacked = false;
    }

    public void SeenByEnemy()
    {
        NumberOfEnemiesAwareOfPlayer++;
    }

    public void EnemyLostPlayer()
    {
        NumberOfEnemiesAwareOfPlayer--;
        if (NumberOfEnemiesAwareOfPlayer < 0)
            NumberOfEnemiesAwareOfPlayer = 0;
    }
}
