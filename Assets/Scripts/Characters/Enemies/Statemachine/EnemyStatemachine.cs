using UnityEngine;

public class EnemyStatemachine
{
    private bool IsCurrentStateLocked => CurrentState.IsStateLocked;
    public EnemyState CurrentState { get; set; }

    public void Initialize(EnemyState startingState)
    {
        CurrentState = startingState;
        CurrentState.OnEnter();
    }

    public void ChangeState(EnemyState newState)
    {
        if (IsCurrentStateLocked) return;
        CurrentState.OnExit();
        CurrentState = newState;
        CurrentState.OnEnter();
    }
}
