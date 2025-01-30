using UnityEngine;
using UnityEngine.AI;

public class DeadState : EnemyState
{
    private float TimeSpentDead = 0f;

    public DeadState(EnemyStatemachine stateMachine, EnemyAI enemy, NavMeshAgent agent) : base(stateMachine, enemy, agent)
    {
    }

    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        NavMeshAgent.isStopped = true;
        NavMeshAgent.destination = NavMeshAgent.transform.position;
    }

    public override void OnExit()
    {
        base.OnExit();
        Enemy.Heal();
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        TimeSpentDead += Time.deltaTime;
        if (TimeSpentDead >= Enemy.ReviveTime)
        {
            StateMachine.ChangeState(Enemy.IdleState);
        }
    }

    public override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();
    }
}
