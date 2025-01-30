using System;
using System.Collections;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    public MeleeAttacks Stats;

    [SerializeField] float HitStopPower = 2;
    [SerializeField] float HitStopTime = 0.01f;

    EnemyAI CachedEnemy;

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
            StartCoroutine(HitStop.TimeSlow(HitStopPower, HitStopTime));
        }
    }
}
