using UnityEngine.AI;
using UnityEngine;

public class AttackState : EnemyState
{
    private float AttackCooldown = 1f; // Time between attacks
    private float TimeSinceLastAttack = 0f;

    public AttackState(EnemyStatemachine stateMachine, EnemyAI enemy, NavMeshAgent agent) : base(stateMachine, enemy, agent) { }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Enemy is attacking the player!");
        AttackPlayer();
        TimeSinceLastAttack = AttackCooldown;
        NavMeshAgent.isStopped = true;
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        TimeSinceLastAttack += Time.deltaTime;

        // Perform attack if cooldown is over
        if (TimeSinceLastAttack >= AttackCooldown)
        {
            TimeSinceLastAttack = 0f;
            AttackPlayer();
        }

        // Check if the player is still within attack range
        float distanceToPlayer = Vector3.Distance(Enemy.transform.position, Enemy.Player.transform.position);
        if (distanceToPlayer > 1.5f)
        {
            StateMachine.ChangeState(Enemy.ChaseState);
        }
    }

    private void AttackPlayer()
    {
        // TODO: Implement attack logic, e.g., reduce player health
        Debug.Log("Attacking the player!");
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
