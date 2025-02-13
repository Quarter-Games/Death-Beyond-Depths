using UnityEngine.AI;
using UnityEngine;

public class AttackState : EnemyState
{
    private float TimeSinceLastAttack = 0f;
    const string ATTACK_ANIMATION = "IsAttacking";
    const string ATTACK_RNG = "AttackRNG";

    public AttackState(EnemyStatemachine stateMachine, EnemyAI enemy, NavMeshAgent agent) : base(stateMachine, enemy, agent) { }

    public override void OnEnter()
    {
        base.OnEnter();
        TimeSinceLastAttack = Enemy.MeleeAttackCooldown;
        NavMeshAgent.isStopped = true;
        NavMeshAgent.SetDestination(Enemy.transform.position);
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        TimeSinceLastAttack += Time.deltaTime;
        // Perform attack if cooldown is over
        if (TimeSinceLastAttack >= Enemy.MeleeAttackCooldown)
        {
            TimeSinceLastAttack = 0f;
            AttackPlayer();
        }
        // Check if the player is still within attack range
        float distanceToPlayer = Vector3.Distance(Enemy.transform.position, Enemy.Player.transform.position);
        if (distanceToPlayer > Enemy.MeleeAttackRange)
        {
            StateMachine.ChangeState(Enemy.ChaseState);
        }
    }

    private void AttackPlayer()
    {
        // TODO: Implement attack logic, e.g., reduce player health
        Debug.Log("Attacking the player!");
        Enemy.Animator.SetFloat(ATTACK_RNG, Random.Range(0, 1f));
        Enemy.Animator.SetTrigger(ATTACK_ANIMATION);
    }

    public override void OnExit()
    {
        base.OnExit();
        Enemy.Animator.ResetTrigger(ATTACK_ANIMATION);
    }
}
