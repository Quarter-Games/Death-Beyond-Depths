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
        Debug.Log("Exiting " + CurrentState);
        CurrentState.OnExit();
        CurrentState = newState;
        Debug.Log("Entering " + CurrentState);
        CurrentState.OnEnter();
        //Debug.Log("Entered " + CurrentState);
    }
}
