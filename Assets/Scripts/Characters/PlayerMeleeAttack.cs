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
        EnemyList = new List<EnemyAI>();
    }

    private void OnDisable()
    {
        foreach (EnemyAI enemy in EnemyList)
        {
            CurrentEnemy.IsAttacked = false;
        }
    }

    private IEnumerator DestroyAfterSeconds(float attackTime)
    {
        yield return new WaitForSeconds(attackTime);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out CurrentEnemy) && !CurrentEnemy.IsAttacked)
        {
            if (CurrentEnemy.IsDead) return;
            CurrentEnemy.TakeDamage(Stats.Damage);
            StartCoroutine(HitStop.TimeSlow(HitStopPower, HitStopTime));
            CurrentEnemy.IsAttacked = true; //to avoid multi-hits when reentering collider
            EnemyList.Add(CurrentEnemy);
        }
    }
}
