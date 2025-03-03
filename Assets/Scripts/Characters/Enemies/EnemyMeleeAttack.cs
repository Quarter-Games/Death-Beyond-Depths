using System.Collections;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    public MeleeAttacks AttackStats;
    PlayerCharacterController CachedPlayer;

    private void OnEnable()
    {
        StartCoroutine(DestroyAfterSeconds(AttackStats.AttackTime));
    }

    private IEnumerator DestroyAfterSeconds(float attackTime)
    {
        yield return new WaitForSeconds(attackTime);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CachedPlayer))
        {
            CachedPlayer.TakeDamage(AttackStats.Damage);
        }
    }
}
