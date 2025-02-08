using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    public MeleeAttacks Stats;

    [SerializeField] float HitStopPower = 2;
    [SerializeField] float HitStopTime = 0.01f;

    List<EnemyAI> EnemyList;
    EnemyAI CurrentEnemy;

    private void OnEnable()
    {
        StartCoroutine(DestroyAfterSeconds(Stats.AttackTime));
        foreach (EnemyAI enemy in EnemyList)
        {
            enemy.stats.IsInvincible = false; //should be something else rather than invincibilty to avoid bugs
        }
    }

    private IEnumerator DestroyAfterSeconds(float attackTime)
    {
        yield return new WaitForSeconds(attackTime);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out CurrentEnemy))
        {
            if (CurrentEnemy.IsDead) return;
            CurrentEnemy.TakeDamage(Stats.Damage);
            StartCoroutine(HitStop.TimeSlow(HitStopPower, HitStopTime));
            CurrentEnemy.stats.IsInvincible = true; //to avoid multi-hits when reentering collider
            EnemyList.Add(CurrentEnemy);
        }
    }
}
