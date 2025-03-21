using UnityEngine;
using UnityEngine.AI;

public class DeadState : EnemyState
{
    public float TimeSpentDead = 0f;
    const string DEATH_ANIMATION = "IsDead";

    private bool isDead = false;

    public DeadState(EnemyStatemachine stateMachine, EnemyAI enemy, NavMeshAgent agent) : base(stateMachine, enemy, agent)
    {
    }

    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
        IsStateLocked = false;
        NavMeshAgent.destination = NavMeshAgent.transform.position;
        StateMachine.ChangeState(Enemy.IdleState);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        if(Enemy.IsKnownPlayerPositionCorrect)
        {
            Enemy.Player.EnemyLostPlayer();
        }
        NavMeshAgent.isStopped = true;
        NavMeshAgent.destination = NavMeshAgent.transform.position;
        Enemy.stats.IsInvincible = true;
        TimeSpentDead = 0;
        Enemy.Animator.SetBool(DEATH_ANIMATION, true);
        isDead = true;
        IsStateLocked = true;
    }

    public override void OnExit()
    {
        base.OnExit();
        Enemy.Heal();
        Enemy.stats.IsInvincible = false;
        //Enemy.Animator.SetBool(DEATH_ANIMATION, false);
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        TimeSpentDead += Time.deltaTime;
        if (isDead && TimeSpentDead >= Enemy.ReviveTime)
        {
            isDead = false;
            TimeSpentDead = 0;
            Enemy.Animator.SetBool(DEATH_ANIMATION, false);
        }
    }

    public override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();
    }
}
