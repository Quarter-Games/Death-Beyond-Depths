using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

abstract public class Character : MonoBehaviour
{
    [SerializeField] float WalkDamping;
    [SerializeField] float RunDamping;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Stats stats;

    protected virtual void OnEnable()
    {
        stats.OnDeath += Die;
    }

    protected virtual void OnDisable()
    {
        stats.OnDeath -= Die;
    }

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

    public void Die()
    {
        //TODO character death stuff, for now disable the gameObject
        gameObject.SetActive(false);
    }
}
[Serializable]
public class Stats
{
    public float MovementSpeed;
    public int HP;
    public int Damage;

    private bool IsInvincible = false;

    public event Action OnHPChanged;
    public event Action OnDeath;
    //TODO make Invinciblity event if needed

    public void TakeDamage(int damage = 1)
    {
        if (damage <= 0 || IsInvincible)
            return;
        HP -= damage;
        OnHPChanged?.Invoke();
        if (HP <= 0)
        {
            OnDeath?.Invoke();
        }
    }
    public IEnumerator BecomeInvincibleForSeconds(float Seconds)
    {
        IsInvincible = true;
        yield return new WaitForSeconds(Seconds);
        IsInvincible = false;
    }
}