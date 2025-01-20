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
            if (CachedEnemy.IsDead) return;
            CachedEnemy.TakeDamage(Stats.Damage);
            Debug.Log("Hit enemy");
            StartCoroutine(HitStop.TimeSlow(3, 0.01f));
        }
    }
}
