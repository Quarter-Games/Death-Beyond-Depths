using UnityEngine.AI;
using UnityEngine;
using System;

public class AlertState : EnemyState
{
    private float AlertDuration = 5f;
    private float TimeSpentInAlert = 0f;
    const string ALERT_ANIMATION = "IsSeeking";
    private float TimeToCheckForPlayer = 0.5f;
    private float TimerOfPlayerCheck = 0.5f;
    bool isPlayerInSight;

    public static event Action<EnemyAI> OnLosingPlayer;

    public AlertState(EnemyStatemachine stateMachine, EnemyAI enemy, NavMeshAgent agent) : base(stateMachine, enemy, agent) { }

    public override void OnEnter()
    {
        base.OnEnter();
        //TODO alert animation
        TimeSpentInAlert = 0f;
        NavMeshAgent.isStopped = false;
        NavMeshAgent.speed = Enemy.AlertMoveSpeed;
        Enemy.Animator.SetBool(ALERT_ANIMATION, true);
        NavMeshAgent.SetDestination(Enemy.LastKnownPlayerPosition);
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        NavMeshAgent.SetDestination(Enemy.LastKnownPlayerPosition);
        // Check if the player is in sight
        TimerOfPlayerCheck += Time.deltaTime;
        if (TimerOfPlayerCheck >= TimeToCheckForPlayer)
        {
            TimerOfPlayerCheck = 0;
            isPlayerInSight = Enemy.PlayerInSight();
            if (isPlayerInSight)
            {
                TimerOfPlayerCheck = 0;
            }
        }
        if (isPlayerInSight)
        {
            StateMachine.ChangeState(Enemy.ChaseState);
            return;
        }
        // Stay alert for a duration, then return to idle if no player is found
        TimeSpentInAlert += Time.deltaTime;
        if (TimeSpentInAlert >= AlertDuration)
        {
            OnLosingPlayer?.Invoke(Enemy);
            StateMachine.ChangeState(Enemy.IdleState);
            return;
        }
        NavMeshAgent.isStopped = false;
        if (Mathf.Abs(NavMeshAgent.velocity.x) <= 0.1f && Enemy.Animator.GetBool(ALERT_ANIMATION))
        {
            Enemy.Animator.SetBool(ALERT_ANIMATION, false);
        }
        else if (Mathf.Abs(NavMeshAgent.velocity.x) >= 0.1f && !Enemy.Animator.GetBool(ALERT_ANIMATION))
        {
            Enemy.Animator.SetBool(ALERT_ANIMATION, true);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        Enemy.Animator.SetBool(ALERT_ANIMATION, false);
    }

    private void CheckHidingSpot()
    {

    }
}
