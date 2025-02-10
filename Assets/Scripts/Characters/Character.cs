using DG.Tweening;
using System;
using System.Collections;
using UnityEditor.Tilemaps;
using UnityEngine;

abstract public class Character : MonoBehaviour
{
    [SerializeField] float WalkDamping;
    [SerializeField] float RunDamping;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] public Stats stats;
    private float animSpeed;

    public bool IsAttacked {  get; set; }

    public Room CurrentRoom { get; set; }

    protected virtual void OnEnable()
    {
        stats.OnDeath += Die;
    }

    protected virtual void OnDisable()
    {
        stats.OnDeath -= Die;
    }

    protected abstract void Flip();

    public void Move(Vector2 direction, MovementMode moveMode = default)
    {
        bool isIdle = false;
        if (direction == Vector2.zero)
        {
            isIdle = true;
        }
        direction = direction.normalized;
        rb.linearDamping = moveMode switch
        {
            MovementMode.Walking => WalkDamping,
            MovementMode.Running => RunDamping,
            _ => WalkDamping
        };
        if (isIdle) animSpeed = 0;
        else animSpeed = moveMode == MovementMode.Walking ? 1f : 2;
        TweenAnimatorSpeedValue();
        rb.AddForce(direction * stats.MovementSpeed);
    }

    private void TweenAnimatorSpeedValue()
    {
        float speed = animator.GetFloat("Speed");
        DOTween.To(() => animator.GetFloat("Speed"), x => animator.SetFloat("Speed", x), animSpeed, 0.1f);

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
    public bool IsInvincible = false;

    public event Action OnHPChanged;
    public event Action OnDeath;
    //TODO make Invinciblity event if needed

    public void TakeDamage(int damage = 1)
    {
        if (damage <= 0 || IsInvincible)
            return;
        HP -= damage;
        if (HP < 0) HP = 0;
        OnHPChanged?.Invoke();
        if (HP <= 0)
        {
            OnDeath?.Invoke();
        }
    }
    public IEnumerator BecomeInvincibleForSeconds(float Seconds)
    {
        IsInvincible = true;
        Debug.Log("invincible for " + Seconds + " seconds");
        yield return new WaitForSeconds(Seconds);
        Debug.Log("no longer invincible");
        IsInvincible = false;
    }
}