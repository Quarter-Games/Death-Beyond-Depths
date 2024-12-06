using DG.Tweening;
using System;
using UnityEngine;

abstract public class Character : MonoBehaviour
{
    [SerializeField] float WalkDamping;
    [SerializeField] float RunDamping;
    [SerializeField] Animator animator;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Stats stats;
    public void Move(Vector2 direction, MovementMode moveMode = default)
    {
        direction = direction.normalized;
        rb.linearDamping = moveMode switch
        {
            MovementMode.Walking => WalkDamping,
            MovementMode.Running => RunDamping,
            _ => WalkDamping
        };
        rb.AddForce(direction * stats.MovementSpeed);
    }
    public enum MovementMode
    {
        Walking,
        Running
    }
}
[Serializable]
public class Stats
{
    public float MovementSpeed;
    public int HP;
    public int Damage;
}