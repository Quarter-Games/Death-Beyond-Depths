using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle State", menuName = "DBD/States/IdleState")]
public class IdleEnemyState : CharacterState
{
    public override void StateEnteredCallback()
    {
        Debug.Log("Idle State Entered");
    }
    public override void StateExitedCallback()
    {
        Debug.Log("Idle State Exited");
    }
}