using UnityEngine;
using UnityEngine.AI;

public class ChargeAttackState : EnemyState
{
    private Vector3 LastKnownPlayerPosition;
    private float TimeSinceLastAttack = 0f;
    private bool IsCharging = false;
    private bool ChargeComplete;
    const string CHARGE_ANIMATION = "IsCharging";

    public ChargeAttackState(EnemyStatemachine stateMachine, EnemyAI enemy, NavMeshAgent agent) : base(stateMachine, enemy, agent) { }

    public override void OnEnter()
    {
        //TODO - Scream animation
        base.OnEnter();
        Debug.Log("Entered charge attack state");
        LastKnownPlayerPosition = Enemy.Player.transform.position;
        AttackPlayer();
        TimeSinceLastAttack = Enemy.ChargeAttackCooldown;
        NavMeshAgent.isStopped = true;
        NavMeshAgent.SetDestination(LastKnownPlayerPosition);
        NavMeshAgent.speed = Enemy.ChargeMoveSpeed;
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        TimeSinceLastAttack += Time.deltaTime;
        if (TimeSinceLastAttack >= Enemy.ChargeAttackCooldown)
        {
            TimeSinceLastAttack = 0f;
            AttackPlayer();
        }
        if (IsCharging || !ChargeComplete) //in the middle of attack, don't change state
        {
            return;
        }
        float distanceToPlayer = Vector3.Distance(Enemy.transform.position, Enemy.Player.transform.position);
        if (distanceToPlayer <= Enemy.MeleeAttackRange)
        {
            StateMachine.ChangeState(Enemy.AttackState);
        }
        else if (distanceToPlayer <= Enemy.ChargeAttackRange)
        {
            StateMachine.ChangeState(Enemy.ChargeAttackState);
        }
        else
        {
            StateMachine.ChangeState(Enemy.ChaseState);
        }
    }

    private void AttackPlayer()
    {
        IsCharging = true;
        ChargeComplete = false;
        Enemy.Animator.SetBool(CHARGE_ANIMATION, true);
        NavMeshAgent.isStopped = false;
        NavMeshAgent.SetDestination(LastKnownPlayerPosition);
        Enemy.StartCoroutine(EndCharge());
    }

    private System.Collections.IEnumerator EndCharge()
    {
        // Wait for the animation to end (adjust timing as needed)
        yield return new WaitForSeconds(Enemy.Animator.GetCurrentAnimatorStateInfo(0).length);
        IsCharging = false;
        ChargeComplete = true;
        NavMeshAgent.isStopped = true;
        NavMeshAgent.destination = NavMeshAgent.transform.position;
        Enemy.Animator.SetBool(CHARGE_ANIMATION, false);
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
