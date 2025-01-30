using UnityEngine;
using UnityEngine.AI;

public class ChaseState : EnemyState
{
    public ChaseState(EnemyStatemachine stateMachine, EnemyAI enemy, NavMeshAgent agent) : base(stateMachine, enemy, agent) { }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Entered chase state");
        NavMeshAgent.isStopped = false;
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
        float distanceToPlayer = Vector3.Distance(Enemy.transform.position, Enemy.LastKnownPlayerPosition.position);
        if(distanceToPlayer <= Enemy.MeleeAttackRange)
        {
            StateMachine.ChangeState(Enemy.AttackState);
            return;
        }
        else if (distanceToPlayer <= Enemy.ChargeAttackRange)
        {
            StateMachine.ChangeState(Enemy.ChargeAttackState);
            return;
        }
        NavMeshAgent.SetDestination(Enemy.LastKnownPlayerPosition.position);
    }

    public override void OnExit()
    {
        base.OnExit();
        NavMeshAgent.isStopped = true;
    }
}

