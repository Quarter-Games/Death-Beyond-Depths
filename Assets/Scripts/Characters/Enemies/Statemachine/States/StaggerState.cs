using System;
using UnityEngine;
using UnityEngine.AI;

public class StaggerState : EnemyState
{
    private float TimeSpentInStagger = 0f;
    const string STAGGER_ANIMATION = "IsGettingHit";

    public StaggerState(EnemyStatemachine stateMachine, EnemyAI enemy, NavMeshAgent agent) : base(stateMachine, enemy, agent)
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
        NavMeshAgent.SetDestination(Enemy.transform.position);
        Enemy.Animator.SetTrigger(STAGGER_ANIMATION);
        Debug.Log("Entered staggered state");
        TimeSpentInStagger = 0;
    }

    public override void OnExit()
    {
        base.OnExit();
        NavMeshAgent.isStopped = false;
        NavMeshAgent.destination = NavMeshAgent.transform.position;
        Enemy.Animator.ResetTrigger(STAGGER_ANIMATION);
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        TimeSpentInStagger += Time.deltaTime;
        if (TimeSpentInStagger >= Enemy.StaggerTime)
        {
            StateMachine.ChangeState(Enemy.AlertState);
        }
    }

    public override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();
    }
}
