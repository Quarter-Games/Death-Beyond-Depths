using UnityEngine;
using UnityEngine.AI;

public class EnemyState
{
    protected EnemyStatemachine StateMachine;
    protected EnemyAI Enemy;
    protected NavMeshAgent NavMeshAgent;

    public EnemyState(EnemyStatemachine stateMachine, EnemyAI enemy, NavMeshAgent agent)
    {
        StateMachine = stateMachine;
        Enemy = enemy;
        NavMeshAgent = agent;
    }

    public virtual void OnEnter() { }
    public virtual void OnFrameUpdate() { }
    public virtual void OnPhysicsUpdate() { }
    public virtual void OnExit() { }
    public virtual void AnimationTriggerEvent() { }
}
