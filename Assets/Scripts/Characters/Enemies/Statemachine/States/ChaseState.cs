using System;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : EnemyState
{
    private float ChaseDuration = 2f;
    private float TimeSpentInChase = 0f;
    bool IsScreaming = false;

    const string DISCOVER_PLAYER_ANIMATION = "FoundPlayer";
    const string SCREAM_CHANCE = "ScreamChance";
    const string CHASE_ANIMATION = "IsChasing";

    public static event Action<EnemyAI> OnSeenPlayer;

    public ChaseState(EnemyStatemachine stateMachine, EnemyAI enemy, NavMeshAgent agent) : base(stateMachine, enemy, agent) { }

    public override void AnimationTriggerEvent()
    {
        StartChase();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        TimeSpentInChase = 0;
        NavMeshAgent.isStopped = false;
        Enemy.Animator.SetFloat(SCREAM_CHANCE, UnityEngine.Random.Range(0, 1f));
        if (Enemy.Animator.GetFloat(SCREAM_CHANCE) > 0.5f)
        {
            IsScreaming = true;
            Enemy.Animator.SetTrigger(DISCOVER_PLAYER_ANIMATION);
            NavMeshAgent.SetDestination(Enemy.transform.position);
            Debug.Log("found player");
        }
        else
        {
            StartChase();
        }
        NavMeshAgent.speed = Enemy.ChaseMoveSpeed;
        OnSeenPlayer?.Invoke(Enemy);
    }

    private void StartChase()
    {
        IsScreaming = false;
        Enemy.Animator.SetBool(CHASE_ANIMATION, true);
        NavMeshAgent.SetDestination(Enemy.LastKnownPlayerPosition);
    }

    public override void OnFrameUpdate()
    {
        base.OnFrameUpdate();
        if (IsScreaming) return;
        NavMeshAgent.SetDestination(Enemy.LastKnownPlayerPosition);
        bool isPlayerInSight = Enemy.PlayerInSight();
        if (!isPlayerInSight)
        {
            TimeSpentInChase += Time.deltaTime;
        }
        else
        {
            TimeSpentInChase = 0;
        }
        if (TimeSpentInChase >= ChaseDuration)
        {
            Debug.Log("leaving to alert");
            StateMachine.ChangeState(Enemy.AlertState);
            return;
        }
        // Check if the player is within attack ranges
        float distanceToPlayer = Vector3.Distance(Enemy.transform.position, Enemy.LastKnownPlayerPosition);
        if (distanceToPlayer <= Enemy.MeleeAttackRange && Enemy.IsKnownPlayerPositionCorrect)
        {
            Debug.Log("leaving to melee");
            StateMachine.ChangeState(Enemy.AttackState);
            return;
        }
        else if (distanceToPlayer <= Enemy.ChargeAttackRange && Enemy.IsKnownPlayerPositionCorrect)
        {
            Debug.Log("leaving to charge");
            StateMachine.ChangeState(Enemy.ChargeAttackState);
            return;
        }
        NavMeshAgent.isStopped = false;
        if (Mathf.Abs(NavMeshAgent.velocity.x) <= 0.1f && Enemy.Animator.GetBool(CHASE_ANIMATION))
        {
            Enemy.Animator.SetBool(CHASE_ANIMATION, false);
        }
        else if (Mathf.Abs(NavMeshAgent.velocity.x) >= 0.1f && !Enemy.Animator.GetBool(CHASE_ANIMATION))
        {
            Enemy.Animator.SetBool(CHASE_ANIMATION, true);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        NavMeshAgent.isStopped = true;
        Enemy.Animator.SetBool(CHASE_ANIMATION, false);
        //Enemy.Animator.ResetTrigger(DISCOVER_PLAYER_ANIMATION);
        IsScreaming = false;
        Debug.Log("Leaving chase");
    }
}

