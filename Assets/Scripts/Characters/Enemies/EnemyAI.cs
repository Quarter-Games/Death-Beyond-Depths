using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : Character, IHearing
{
    [Range(0f, 360f), SerializeField] float Angle = 45f;
    [SerializeField] float SightRadius = 5f;
    [SerializeField] float SoundRadius = 5f;
    [SerializeField] NavMeshAgent NavMeshAgent;
    [SerializeField] List<Transform> PatrolPoints;
    [SerializeField] public PlayerCharacterController Player;
    [SerializeField] public float StaggerTime = 0.5f;
    [SerializeField] public float ReviveTime = 10f;
    [Header("Attack Stats")]
    [SerializeField] public float ChargeAttackRange = 4;
    [SerializeField] public float MeleeAttackRange = 2;
    [SerializeField] public float MeleeAttackCooldown = 0.4f;
    [SerializeField] public float ChargeAttackCooldown = 0.4f;
    [SerializeField] public int ChargeAttackDamage = 5;
    [SerializeField] public int MeleeAttackDamage = 5;
    [Header("Move Speed")]
    [SerializeField] public float PatrolMoveSpeed = 3f;
    [SerializeField] public float AlertMoveSpeed = 4f;
    [SerializeField] public float ChaseMoveSpeed = 5f;
    [SerializeField] public float ChargeMoveSpeed = 6f;

    public Vector3 LastKnownPlayerPosition { get; set; }
    public IdleWanderState IdleState { get; private set; }
    public AlertState AlertState { get; private set; }
    public ChaseState ChaseState { get; private set; }
    public AttackState AttackState { get; private set; }
    public ChargeAttackState ChargeAttackState { get; private set; }
    public AttackDoorState AttackDoorState { get; private set; }
    public StaggerState StaggerState { get; private set; }
    public DeadState DeadState { get; private set; }
    public Animator Animator { get; private set; }
    public bool IsDead => stats.HP <= 0;
    public bool IsAwareOfPlayer => StateMachine.CurrentState == ChaseState || StateMachine.CurrentState == ChargeAttackState || StateMachine.CurrentState == AttackState;

    EnemyStatemachine StateMachine;
    private bool IsFacingLeft = false;
    private Vector3 DefaultScale;
    private int InitialHP;

    protected override void OnEnable() {}

    protected override void OnDisable(){}

    private void Awake()
    {
        InitializeNavMeshAgent();
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        StateMachine = new();
        IdleState = new IdleWanderState(StateMachine, this, NavMeshAgent, PatrolPoints);
        AlertState = new AlertState(StateMachine, this, NavMeshAgent);
        ChaseState = new ChaseState(StateMachine, this, NavMeshAgent);
        AttackState = new AttackState(StateMachine, this, NavMeshAgent);
        ChargeAttackState = new ChargeAttackState(StateMachine, this, NavMeshAgent);
        StaggerState = new StaggerState(StateMachine, this, NavMeshAgent);
        DeadState = new DeadState(StateMachine, this, NavMeshAgent);
        AttackDoorState = new AttackDoorState(StateMachine, this, NavMeshAgent);
    }

    private void InitializeNavMeshAgent()
    {
        NavMeshAgent.updateRotation = false;
        NavMeshAgent.speed = stats.MovementSpeed;
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
        DefaultScale = transform.localScale;
        InitialHP = stats.HP;
        Animator = animator;
    }

    private void Update()
    {
        StateMachine.CurrentState.OnFrameUpdate();
        if (!IsDead && stats.HP < InitialHP && CanHeal())
        {
            Heal();
            return;
        }
        else if (StateMachine.CurrentState == DeadState)
        {
            return;
        }
        else if (IsDead)
        {
            StateMachine.ChangeState(DeadState);
        }
        FlipIfNeeded();
    }

    private void FlipIfNeeded()
    {
        if (NavMeshAgent.velocity.x > 0 && IsFacingLeft)
        {
            IsFacingLeft = false;
            Flip();
        }
        else if (NavMeshAgent.velocity.x < 0 && !IsFacingLeft)
        {
            IsFacingLeft = true;
            Flip();
        }
    }

    private bool CanHeal()
    {
        return false;
    }

    public void Heal()
    {
        stats.HP = InitialHP;
    }

    public void TakeDamage(int damage)
    {
        stats.TakeDamage(damage);
        if (stats.IsInvincible) return;
        StateMachine.ChangeState(StaggerState);
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.OnPhysicsUpdate();
    }

    protected override void Flip()
    {
        transform.localScale = new Vector3(
            IsFacingLeft ? -Mathf.Abs(DefaultScale.x) : Mathf.Abs(DefaultScale.x),
            DefaultScale.y,
            DefaultScale.z
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out EnemyAI otherEnemy))
        {
            if(StateMachine.CurrentState == IdleState && otherEnemy.StateMachine.CurrentState == IdleState)
            {
                //TODO reverse directions for both enemies
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Door door))
        {
            if(IsAwareOfPlayer) // TODO - check if player is behind the door
            {
                AttackDoorState.Door = door;
                StateMachine.ChangeState(AttackDoorState);
                return;
            }
        }
        if (collision.gameObject.TryGetComponent(out HiddenArea area))
        {
            if(StateMachine.CurrentState != AlertState) return;
            if (UnityEngine.Random.Range(0, 100) == 0)
            {
                area.UnHidePlayer();
                return;
            }
            if (!area.IsPlayerHiddenInside) return;
        }
    }

    public bool PlayerInSight()
    {
        if (Player == null || Player.IsHidden)
            return false;
        Vector3 directionToPlayer = (Player.transform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
        if (distanceToPlayer > SightRadius)
        {
            return false;
        }
        Vector3 facingDirection = IsFacingLeft ? -transform.right : transform.right;
        float angleToPlayer = Vector3.Angle(facingDirection, directionToPlayer);
        if (angleToPlayer <= Angle / 2f)
        {
            LastKnownPlayerPosition = Player.transform.position;
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, SoundRadius);

        Vector3 leftBoundary = IsFacingLeft ? Quaternion.Euler(0, 0, Angle / 2) * transform.right * -1 : Quaternion.Euler(0, 0, Angle / 2) * transform.right;
        Vector3 rightBoundary = IsFacingLeft ? Quaternion.Euler(0, 0, -Angle / 2) * transform.right * -1 : Quaternion.Euler(0, 0, -Angle / 2) * transform.right;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, leftBoundary * SightRadius);
        Gizmos.DrawRay(transform.position, rightBoundary * SightRadius);

        if (PatrolPoints.Count == 0) return;
        int i = 0;
        Gizmos.color = Color.yellow;
        for (; i < PatrolPoints.Count - 1;)
        {
            Gizmos.DrawSphere(PatrolPoints[i].position, 0.2f);
            Gizmos.DrawLine(PatrolPoints[i].position, PatrolPoints[++i].position);
        }
        Gizmos.DrawSphere(PatrolPoints[i].position, 0.2f);
    }

    public void OnHeardSound(Vector3 soundOrigin)
    {
        if (StateMachine.CurrentState != IdleState && StateMachine.CurrentState != AlertState)
        {
            return;
        }
        Debug.Log("Davy heard that!");
        LastKnownPlayerPosition = soundOrigin;
        StateMachine.ChangeState(AlertState);
    }
}
