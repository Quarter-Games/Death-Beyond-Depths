using UnityEngine;
using UnityEngine.AI;

public class AttackDoorState : EnemyState
{
    private float TimeSinceLastAttack = 0f;
    const string ATTACK_ANIMATION = "IsAttacking";

    public Door Door { get; set; }

    public AttackDoorState(EnemyStatemachine stateMachine, EnemyAI enemy, NavMeshAgent agent) : base(stateMachine, enemy, agent)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Entered door attack state");
        NavMeshAgent.SetDestination(Enemy.transform.position);
        TimeSinceLastAttack = Enemy.MeleeAttackCooldown;
    }

    public override void OnExit()
    {
        base.OnExit();
        Enemy.Animator.ResetTrigger(ATTACK_ANIMATION);
    }

    private void AttackDoor()
    {
        Debug.Log("Attacking the door");
        Enemy.Animator.SetTrigger(ATTACK_ANIMATION);
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        TimeSinceLastAttack += Time.deltaTime;
        // Perform attack if cooldown is over
        if (TimeSinceLastAttack >= Enemy.MeleeAttackCooldown)
        {
            TimeSinceLastAttack = 0f;
            AttackDoor();
        }
        if (Door != null && Door.IsBroken)
        {
            StateMachine.ChangeState(Enemy.AlertState);
            return;
        }

    }

    public override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();
    }
}
