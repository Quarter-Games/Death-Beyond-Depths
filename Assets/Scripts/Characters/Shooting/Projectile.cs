using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed;
    public Rigidbody2D rb;
    public RangeAttack Stats;
    Enemy CachedEnemy;
    public void Init(Vector2 direction)
    {
        rb.linearVelocity = direction * Speed;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CachedEnemy))
        {
            CachedEnemy.TakeDamage(Stats.Damage);
            Destroy(gameObject);
        }

    }
}
