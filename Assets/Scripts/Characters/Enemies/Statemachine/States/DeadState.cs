using UnityEngine;
using UnityEngine.AI;

public class DeadState : EnemyState
{
    private float TimeSpentDead = 0f;
    const string DEATH_ANIMATION = "IsDead";

    public DeadState(EnemyStatemachine stateMachine, EnemyAI enemy, NavMeshAgent agent) : base(stateMachine, enemy, agent)
    {
    }

    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
        IsStateLocked = false;
        StateMachine.ChangeState(Enemy.IdleState);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Entered dead state");
        NavMeshAgent.isStopped = true;
        NavMeshAgent.destination = NavMeshAgent.transform.position;
        Enemy.stats.IsInvincible = true;
        TimeSpentDead = 0;
        Enemy.Animator.SetBool(DEATH_ANIMATION, true);
        IsStateLocked = true;
    }

    public override void OnExit()
    {
        base.OnExit();
        Enemy.Heal();
        Enemy.stats.IsInvincible = false;
        Enemy.Animator.SetBool(DEATH_ANIMATION, false);
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        TimeSpentDead += Time.deltaTime;
        if (TimeSpentDead >= Enemy.ReviveTime)
        {
            OnExit();
            //StateMachine.ChangeState(Enemy.IdleState);
        }
    }

    public override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();
    }
}
