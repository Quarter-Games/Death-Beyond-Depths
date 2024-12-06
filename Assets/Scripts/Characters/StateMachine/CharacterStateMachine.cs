using System;
using UnityEngine;

[Serializable]
public class CharacterStateMachine
{
    public delegate void StateChange(CharacterState oldState, CharacterState newState);
    /// <summary>
    /// Called before any state Change
    /// </summary>
    public StateChange OnBeforeStateChange;
    /// <summary>
    /// Called after state change and after animator is called
    /// </summary>
    public StateChange OnEndStateChange;
    [SerializeField] CharacterState _state;
    [SerializeField] Animator _animator;

    /// <summary>
    /// Call in Awake
    /// </summary>
    /// <param name="initialState"></param>
    public CharacterStateMachine(CharacterState initialState,Animator animator)
    {
        _state = initialState;
        _animator = animator;
    }
    public bool TryChangeState(CharacterState newState)
    {
        if (_state.PossibleStates.Contains(newState))
        {
            ChangeState(newState);
            return true;
        }
        return false;
    }
    private void ChangeState(CharacterState newState)
    {
        var prevState = _state;
        OnBeforeStateChange?.Invoke(prevState, newState);
        _state.StateExitedCallback();
        _state = newState;
        _state.StateEnteredCallback();
        CallAnimator();
        OnEndStateChange?.Invoke(prevState, _state);
    }
    /// <summary>
    /// Forces State Change
    /// </summary>
    /// <param name="newState"></param>
    public void ForceChangeState(CharacterState newState)
    {
        ChangeState(newState);
    }
    public void CallAnimator(int integerData = default, float floatData = default, bool booleanData = default)
    {
        if (_animator == null) return;
        switch (_state.AnimatorData.Type)
        {
            case AnimatorParameter.ParameterType.Bool:
                _animator.SetBool(_state.AnimatorData.Name, booleanData == default ? _state.AnimatorData.boolValue : booleanData);
                break;
            case AnimatorParameter.ParameterType.Float:
                _animator.SetFloat(_state.AnimatorData.Name, floatData == default ? _state.AnimatorData.floatValue : floatData);
                break;
            case AnimatorParameter.ParameterType.Int:
                _animator.SetInteger(_state.AnimatorData.Name, integerData == default ? _state.AnimatorData.intValue : integerData);
                break;
            case AnimatorParameter.ParameterType.Trigger:
                _animator.SetTrigger(_state.AnimatorData.Name);
                break;
        }
    }

}
/// <summary>
/// Defines Animator Parameter to be set when this state is active
/// </summary>
[Serializable]
public struct AnimatorParameter
{
    public string Name;
    public ParameterType Type;
    public int intValue;
    public float floatValue;
    public bool boolValue;

    public enum ParameterType
    {
        Bool,
        Float,
        Int,
        Trigger
    }
}
