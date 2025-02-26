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
    [SerializeField] float StaggerChance = 0.25f;

    List<EnemyAI> EnemyList;
    EnemyAI CurrentEnemy;

    private void Start()
    {
        EnemyList = new List<EnemyAI>();
    }

    public void ResetEnemyAttackedList()
    {
        foreach (EnemyAI enemy in EnemyList)
        {
            enemy.IsAttacked = false;
        }
        EnemyList.Clear();
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
            if (UnityEngine.Random.Range(0, 1f) <= StaggerChance)
            {
                CurrentEnemy.Stagger();
                Debug.Log("Staggered");
            }
        }
    }
}
