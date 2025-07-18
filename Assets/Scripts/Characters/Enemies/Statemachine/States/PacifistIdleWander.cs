using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PacifistIdleWander : EnemyState
{
    List<Transform> WayPoints;
    private int Counter = 0;
    private bool ReachedGoal = false;
    Coroutine WaitCoroutine;
    const string WANDER_ANIMATION = "IsWalking";

    public PacifistIdleWander(EnemyStatemachine stateMachine, EnemyAI enemy, NavMeshAgent agent, List<Transform> WanderPoints = null) : base(stateMachine, enemy, agent)
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
        Enemy.Animator.SetBool("DeadAwake", false);
        NavMeshAgent.isStopped = true;
        NavMeshAgent.updatePosition = true;
        NavMeshAgent.ResetPath();
        NavMeshAgent.speed = Enemy.PatrolMoveSpeed;
        NavMeshAgent.destination = NavMeshAgent.transform.position;
        if (WayPoints.Count > 0) Enemy.StartCoroutine(StartWanderAnimation());
    }

    private IEnumerator StartWanderAnimation()
    {
        yield return new WaitForEndOfFrame();
        Enemy.Animator.SetBool(WANDER_ANIMATION, true);
    }

    public override void OnExit()
    {
        base.OnExit();
        NavMeshAgent.isStopped = true;
        Enemy.Animator.SetBool(WANDER_ANIMATION, false);
        if (WaitCoroutine != null) Enemy.StopCoroutine(WaitCoroutine);
        ReachedGoal = false;
        WaitCoroutine = null;
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        if (Mathf.Abs(NavMeshAgent.velocity.x) <= 0.1f && Enemy.Animator.GetBool(WANDER_ANIMATION))
        {
            Enemy.Animator.SetBool(WANDER_ANIMATION, false);
        }
        else if (Mathf.Abs(NavMeshAgent.velocity.x) >= 0.1f && !Enemy.Animator.GetBool(WANDER_ANIMATION))
        {
            Enemy.Animator.SetBool(WANDER_ANIMATION, true);
        }
        if (WayPoints.Count == 0)
        {
            Debug.Log("<color=cyan> no waypoints, enemy is idle</color>", Enemy);
            return; // No waypoints to patrol
        }
        if (!ReachedGoal)
        {
            if (WayPoints[Counter] != null)
            {
                NavMeshAgent.SetDestination(WayPoints[Counter].position);
            }
        }
        Patrol();
    }

    private void Patrol()
    {
        float Distance = MathF.Abs(NavMeshAgent.transform.position.x - WayPoints[Counter].position.x);
        if (Distance <= NavMeshAgent.stoppingDistance)
        {
            ReachedGoal = true;
            if (WaitCoroutine != null) return;
            WaitCoroutine = Enemy.StartCoroutine(WaitAtWayPoint());
        }
    }

    private IEnumerator WaitAtWayPoint()
    {
        Enemy.Animator.SetBool(WANDER_ANIMATION, false);
        yield return new WaitForSeconds(2);
        Enemy.Animator.SetBool(WANDER_ANIMATION, true);
        Counter = (Counter + 1) % WayPoints.Count; // Loop back to the start when reaching the end
        ReachedGoal = false;
        WaitCoroutine = null;
    }

    public override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();
    }
}
