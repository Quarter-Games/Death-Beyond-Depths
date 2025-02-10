using UnityEngine;
using UnityEngine.AI;

public class ChaseState : EnemyState
{
    private float AttackRange = 1.5f;

    public ChaseState(EnemyStatemachine stateMachine, EnemyAI enemy, NavMeshAgent agent) : base(stateMachine, enemy, agent) { }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Enemy is now chasing the player!");
        NavMeshAgent.isStopped = false;
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        // Check if the player is within attack range
        float distanceToPlayer = Vector3.Distance(Enemy.transform.position, Enemy.Player.transform.position);
        if (distanceToPlayer <= AttackRange)
        {
            StateMachine.ChangeState(Enemy.AttackState);
            return;
        }
        if (Enemy.PlayerInSight())
        {
            NavMeshAgent.SetDestination(Enemy.Player.transform.position);
        }
        else
        {
            StateMachine.ChangeState(Enemy.AlertState);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        NavMeshAgent.isStopped = true;
    }
}

