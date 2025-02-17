using UnityEngine.AI;
using UnityEngine;

public class AlertState : EnemyState
{
    private float AlertDuration = 5f;
    private float TimeSpentInAlert = 0f;
    const string ALERT_ANIMATION = "IsSeeking";

    public AlertState(EnemyStatemachine stateMachine, EnemyAI enemy, NavMeshAgent agent) : base(stateMachine, enemy, agent) { }

    public override void OnEnter()
    {
        base.OnEnter();
        //TODO alert animation
        TimeSpentInAlert = 0f;
        NavMeshAgent.isStopped = false;
        NavMeshAgent.speed = Enemy.AlertMoveSpeed;
        Enemy.Animator.SetBool(ALERT_ANIMATION, true);
        //Debug.Log("Last known player position: " + Enemy.LastKnownPlayerPosition);
        //Debug.Log("ACTUAL player position: " + Enemy.Player.transform.position);
        NavMeshAgent.SetDestination(Enemy.LastKnownPlayerPosition);
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
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
        if(NavMeshAgent.velocity.x == 0f)
        {
            Enemy.Animator.SetBool(ALERT_ANIMATION, false);
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
