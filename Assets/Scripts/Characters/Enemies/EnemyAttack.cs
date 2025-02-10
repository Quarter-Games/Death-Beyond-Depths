using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] MeleeAttacks AttackStats;
    PlayerCharacterController CachedPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CachedPlayer))
        {
            CachedPlayer.stats.TakeDamage(AttackStats.Damage);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out CachedPlayer))
        {
            CachedPlayer.stats.TakeDamage(AttackStats.Damage);
        }
    }
}
