using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttacks", menuName = "Scriptable Objects/MeleeAttacks")]
public class MeleeAttacks : ScriptableObject
{
    public float AttackTime;
    public float AttackSize; //TODO is this necessary?
    public int Damage;
    public float CooldownTime;
}
