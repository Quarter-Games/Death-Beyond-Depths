using System;
using System.Collections;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    public MeleeAttacks Stats;
    Enemy CachedEnemy;

    private void OnEnable()
    {
        StartCoroutine(DestroyAfterSeconds(Stats.AttackTime));
    }

    private IEnumerator DestroyAfterSeconds(float attackTime)
    {
        yield return new WaitForSeconds(attackTime);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out CachedEnemy))
        {
            CachedEnemy.TakeDamage(Stats.Damage);
        }
    }
}
