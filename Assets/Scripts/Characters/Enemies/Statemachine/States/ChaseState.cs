using UnityEngine;
using UnityEngine.AI;

public class ChaseState : EnemyState
{
    const string DISCOVER_PLAYER_ANIMATION = "FoundPlayer";
    const string CHASE_ANIMATION = "IsChasing";

    bool IsScreaming = false;

    public ChaseState(EnemyStatemachine stateMachine, EnemyAI enemy, NavMeshAgent agent) : base(stateMachine, enemy, agent) { }

    public override void AnimationTriggerEvent()
    {
        IsScreaming = false;
        NavMeshAgent.SetDestination(Enemy.LastKnownPlayerPosition);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        NavMeshAgent.isStopped = false;
        if (Random.Range(0, 1) > 0.5f)
        {
            IsScreaming = true;
            Enemy.Animator.SetTrigger(DISCOVER_PLAYER_ANIMATION);
            NavMeshAgent.SetDestination(Enemy.transform.position);
        }
        else
        {
            Enemy.Animator.SetBool(CHASE_ANIMATION, true);
            NavMeshAgent.SetDestination(Enemy.LastKnownPlayerPosition);
        }
        NavMeshAgent.speed = Enemy.ChaseMoveSpeed;
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        bool isPlayerInSight = Enemy.PlayerInSight();
        if (!isPlayerInSight)
        {
            StateMachine.ChangeState(Enemy.AlertState);
            return;
        }
        // Check if the player is within attack ranges
        float distanceToPlayer = Vector3.Distance(Enemy.transform.position, Enemy.LastKnownPlayerPosition);
        if (distanceToPlayer <= Enemy.MeleeAttackRange)
        {
            StateMachine.ChangeState(Enemy.AttackState);
            return;
        }
        else if (distanceToPlayer <= Enemy.ChargeAttackRange)
        {
            StateMachine.ChangeState(Enemy.ChargeAttackState);
            return;
        }
        NavMeshAgent.SetDestination(Enemy.LastKnownPlayerPosition);
        NavMeshAgent.isStopped = false;
        if (Mathf.Abs(NavMeshAgent.velocity.x) <= 0.1f && Enemy.Animator.GetBool(CHASE_ANIMATION))
        {
            Enemy.Animator.SetBool(CHASE_ANIMATION, false);
        }
        else if (Mathf.Abs(NavMeshAgent.velocity.x) >= 0.1f && !Enemy.Animator.GetBool(CHASE_ANIMATION))
        {
            Enemy.Animator.SetBool(CHASE_ANIMATION, true);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        NavMeshAgent.isStopped = true;
        Enemy.Animator.SetBool(CHASE_ANIMATION, false);
    }
}

