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

    static List<EnemyAI> EnemyList = new List<EnemyAI>();
    EnemyAI CurrentEnemy;

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
            EnemyList.Add(CurrentEnemy);
            CurrentEnemy.TakeDamage(Stats.Damage);
            StartCoroutine(HitStop.TimeSlow(HitStopPower, HitStopTime));
            CurrentEnemy.IsAttacked = true; //to avoid multi-hits when reentering collider
            CameraController.Instance.ShakeCamera();
            if (UnityEngine.Random.Range(0, 1f) <= StaggerChance)
            {
                CurrentEnemy.Stagger();
            }
        }
    }
}
