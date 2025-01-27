using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : Character
{
    [Range(0f, 360f), SerializeField] float Angle = 45f;
    [SerializeField] float SightRadius = 5f;
    [SerializeField] float SoundRadius = 5f;
    [SerializeField] NavMeshAgent NavMeshAgent;
    [SerializeField] List<Transform> PatrolPoints;
    [SerializeField] public PlayerCharacterController Player;

    public IdleWanderState IdleState { get; private set; }
    public AlertState AlertState { get; private set; }
    public ChaseState ChaseState { get; private set; }
    public AttackState AttackState { get; private set; }

    EnemyStatemachine StateMachine;
    private bool IsFacingLeft;

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
    }

    private void InitializeNavMeshAgent()
    {
        NavMeshAgent.updateRotation = false;
        NavMeshAgent.speed = stats.MovementSpeed;
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentState.OnFrameUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.OnPhysicsUpdate();
    }

    protected override void Flip()
    {
        throw new System.NotImplementedException();
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

    public bool PlayerInSight()
    {
        Vector3 directionToPlayer = IsFacingLeft ? Vector3.left : Vector3.right;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer <= Angle / 2f && Vector3.Distance(transform.position, Player.transform.position) <= SightRadius && !Player.IsHidden)
        {
            if (!Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, SightRadius))
            {
                return false;
            }
            return hit.collider.gameObject == Player.gameObject;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, SoundRadius);

        Vector3 leftBoundary = Quaternion.Euler(0, 0, -Angle / 2) * transform.right;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, Angle / 2) * transform.right;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, leftBoundary * SightRadius);
        Gizmos.DrawRay(transform.position, rightBoundary * SightRadius);
    }

}
