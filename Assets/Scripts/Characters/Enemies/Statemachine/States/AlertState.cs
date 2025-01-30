using UnityEngine.AI;
using UnityEngine;

public class AlertState : EnemyState
{
    private float AlertDuration = 5f;
    private float TimeSpentInAlert = 0f;

    public AlertState(EnemyStatemachine stateMachine, EnemyAI enemy, NavMeshAgent agent) : base(stateMachine, enemy, agent) { }

    public override void OnEnter()
    {
        base.OnEnter();
        //TODO alert animation
        Debug.Log("Entered alert state");
        TimeSpentInAlert = 0f;
        NavMeshAgent.isStopped = true;
        NavMeshAgent.speed = Enemy.AlertMoveSpeed;
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        NavMeshAgent.SetDestination(Enemy.LastKnownPlayerPosition.position);
        // Check if the player is in sight
        if (Enemy.PlayerInSight())
        {
            StateMachine.ChangeState(Enemy.ChaseState);
            return;
        }
        // Stay alert for a duration, then return to idle if no player is found
        TimeSpentInAlert += Time.deltaTime;
        if (TimeSpentInAlert >= AlertDuration)
        {
            StateMachine.ChangeState(Enemy.IdleState);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private void CheckHidingSpot()
    {

    }
}
