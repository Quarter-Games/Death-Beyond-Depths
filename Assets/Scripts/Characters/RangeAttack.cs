using UnityEngine;

[CreateAssetMenu(fileName = "RangeAttack", menuName = "Scriptable Objects/Range Attack")]
public class RangeAttack : ScriptableObject
{
    public float AttackTime;
    public float AttackSize; //TODO is this necessary?
    public int Damage;
    public float CooldownTime;
}
