using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleWanderState : EnemyState
{
    List<Transform> WayPoints;
    private int Counter = 0;
    private bool ReachedGoal = false;

    public IdleWanderState(EnemyStatemachine stateMachine, EnemyAI enemy, NavMeshAgent agent, List<Transform> WanderPoints = null) : base(stateMachine, enemy, agent)
    {
        WayPoints = WanderPoints ?? new List<Transform>();
    }

    public override void AnimationTriggerEvent()
    {
        base.AnimationTriggerEvent();
    }

    public override void OnEnter()
    {
        //TODO - enter idle animation state
        NavMeshAgent.isStopped = true;
        NavMeshAgent.updatePosition = true;
        NavMeshAgent.ResetPath();
    }

    public override void OnExit()
    {
        base.OnExit();
        NavMeshAgent.isStopped = true;
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        if (Enemy.PlayerInSight())
        {
            Debug.Log("Player detected! Transitioning to AlertState.");
            StateMachine.ChangeState(Enemy.AlertState);
            return;
        }
        if (WayPoints.Count == 0) return; // No waypoints to patrol
        if (!ReachedGoal)
        {
            NavMeshAgent.SetDestination(WayPoints[Counter].position);
        }
        Patrol();
    }

    private void Patrol()
    {
        float Distance = MathF.Abs(NavMeshAgent.transform.position.x - WayPoints[Counter].position.x);
        if (Distance <= NavMeshAgent.stoppingDistance)
        {
            ReachedGoal = true;
            Counter = (Counter + 1) % WayPoints.Count; // Loop back to the start when reaching the end
            ReachedGoal = false;
        }
    }

    public override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();
    }
}
