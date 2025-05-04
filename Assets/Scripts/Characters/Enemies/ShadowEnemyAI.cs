using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;

public class ShadowEnemyAI : EnemyAI
{
    PacifistIdleWander PacifistIdleWander { get; set; }

    private void Awake()
    {
        InitializeNavMeshAgent();
        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        StateMachine = new();
        PacifistIdleWander = new PacifistIdleWander(StateMachine, this, NavMeshAgent, PatrolPoints);
    }

    private void InitializeNavMeshAgent()
    {
        NavMeshAgent.updateRotation = false;
        NavMeshAgent.speed = stats.MovementSpeed;
    }

    private void Start()
    {
        DefaultScale = transform.localScale;
        Animator = animator;
        StateMachine.Initialize(PacifistIdleWander);
    }

    private void Update()
    {
        FlipIfNeeded();
        StateMachine.CurrentState.OnFrameUpdate();
    }

    private void FlipIfNeeded()
    {
        if (NavMeshAgent.velocity.x > 0.1f && IsFacingLeft)
        {
            IsFacingLeft = false;
            Flip();
        }
        else if (NavMeshAgent.velocity.x < -0.1f && !IsFacingLeft)
        {
            IsFacingLeft = true;
            Flip();
        }
    }

    protected override void Flip()
    {
        transform.localScale = new Vector3(
            IsFacingLeft ? -Mathf.Abs(DefaultScale.x) : Mathf.Abs(DefaultScale.x),
            DefaultScale.y,
            DefaultScale.z
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
