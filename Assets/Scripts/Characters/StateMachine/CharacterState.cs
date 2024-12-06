using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
abstract public class CharacterState : ScriptableObject
{
    public List<CharacterState> PossibleStates;
    [Tooltip("Animator Parameter to be set when this _state is active")]
    public AnimatorParameter AnimatorData;
    /// <summary>
    /// Called when State Machine is succesfully changed to this state
    /// Do not throw exceptions here
    /// </summary>
    abstract public void StateEnteredCallback();
    /// <summary>
    /// Called when State Machine is changed to another state
    /// Do not throw exceptions here
    /// </summary>
    abstract public void StateExitedCallback();
}
