using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : Character
{
    [Range(0f, 360f), SerializeField] public float Angle = 45f;
    [SerializeField] public float Radius = 5f;
    [SerializeField] float AttackSpeedInSeconds = 2f;
    [SerializeField] float DelayAfterSpotInSeconds = 1f;
    [SerializeField] float ReviveTimeInSeconds = 10f;
    [SerializeField] PlayerCharacterController Player;
    [SerializeField] EnemyMeleeAttack MeleeAttackObject;
    [Header("NavAgent Stats")]
    [SerializeField] NavMeshAgent NavAgent;
    [SerializeField] List<Transform> WayPoints;
    [Tooltip("Object will stop at this distance from way point")]
    [SerializeField, Range(0, 1)] float DistanceFromWayPoint = 0;
    [SerializeField] float AttackRange;
    [SerializeField, Min(0)] float WaitingTime;

    private int InitialHP;
    private int Counter;
    private bool ReachedGoal = false;
    private bool ChasingPlayer = false;
    private bool HasUpdatedWayPoint = false;
    private bool IsGoingBackwards = false;
    private float Distance;

    private Coroutine PatrolCoroutine;
    private Coroutine PlayerCoroutine;
    private bool IsAttacking;

    public bool CanPatrol => WayPoints.Count > 0 && !ChasingPlayer;

    private void OnValidate()
    {
        NavAgent = GetComponent<NavMeshAgent>();
    }

    protected override void OnEnable()
    {
        //base.OnEnable();
        stats.OnDeath += ReviveAndResetEnemy;
    }

    protected override void OnDisable()
    {
        //base.OnDisable();
        stats.OnDeath -= ReviveAndResetEnemy;
    }

    private void Start()
    {
        InitialHP = stats.HP;
        NavAgent.updateRotation = false;
        NavAgent.stoppingDistance = AttackRange;
        
    }

    private void ReviveAndResetEnemy()
    {
        StartCoroutine(Revive());
    }

    private IEnumerator Revive()
    {
        yield return new WaitForSeconds(ReviveTimeInSeconds);
        stats.HP = InitialHP;
        gameObject.SetActive(true);
        if (CanPatrol)
        {
            NavAgent.SetDestination(WayPoints[0].position);
        }
    }

    private IEnumerator WaitAtPosition()
    {
        UpdateToNextWayPoint();
        yield return new WaitForSeconds(WaitingTime);
        ReachedGoal = false;
        HasUpdatedWayPoint = false;
    }

    protected virtual void Update()
    {
        if (IsPlayerInVisionCone())
        {
            Debug.Log("Player spotted!");
            if (PlayerCoroutine == null)
                PlayerCoroutine = StartCoroutine(FollowAndAttackPlayer());
        }
        else
        {
            if (PlayerCoroutine != null)
            {
                StopCoroutine(PlayerCoroutine);
                PlayerCoroutine = null;
            }

            if (CanPatrol && PatrolCoroutine == null)
            {
                Patrol();
            }
        }
    }

    private bool IsPlayerInVisionCone()
    {
        if (Player == null)
            return false;
        Vector3 directionToPlayer = Player.transform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        if (distanceToPlayer > Radius)
            return false;
        directionToPlayer.Normalize();
        float angleToPlayer = Vector3.Angle(transform.right, directionToPlayer);
        ChasingPlayer = angleToPlayer <= Angle / 2f;
        return ChasingPlayer;
    }

    private IEnumerator FollowAndAttackPlayer()
    {
        if (PlayerCoroutine != null)
            yield break;

        ChasingPlayer = true;
        NavAgent.isStopped = true;
        yield return new WaitForSeconds(1);
        NavAgent.isStopped = false;
        while (IsPlayerInVisionCone())
        {
            NavAgent.SetDestination(Player.transform.position);

            float distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
            if (distanceToPlayer <= AttackRange)
            {
                AttackPlayer();
                yield return new WaitForSeconds(AttackSpeedInSeconds);
            }
            else
            {
                yield return null;
            }
        }

        ChasingPlayer = false;
        if (CanPatrol && PatrolCoroutine == null)
        {
            PatrolCoroutine = StartCoroutine(WaitAtPosition());
        }
    }


    private void AttackPlayer()
    {
        IsAttacking = true;
        StartCoroutine(WaitForCoolDown(MeleeAttackObject.AttackStats.CooldownTime));
        MeleeAttackObject.gameObject.SetActive(true);
    
    }
    protected IEnumerator WaitForCoolDown(float time)
    {
        var startTimeTime = Time.time;
        yield return new WaitUntil(() => Time.time - startTimeTime > time);
        IsAttacking = false;
    }

    private void Patrol()
    {
        if (WayPoints == null || WayPoints.Count <= 1 || ChasingPlayer)
            return;
        StopAtDistanceFromWayPoint();
        if (!ReachedGoal)
        {
            NavAgent.SetDestination(WayPoints[Counter].position);
        }
        else if (!HasUpdatedWayPoint)
        {
            StartCoroutine(WaitAtPosition());
        }
    }

    private void StopAtDistanceFromWayPoint()
    {
        Distance = Vector2.Distance(transform.position, WayPoints[Counter].position);
        if (Distance <= NavAgent.stoppingDistance)
            ReachedGoal = true;
    }

    private void UpdateToNextWayPoint()
    {
        if (HasUpdatedWayPoint || ChasingPlayer)
            return;
        if (!IsGoingBackwards)
            Counter++;
        else
            Counter--;
        if (Counter >= WayPoints.Count)
        {
            // if object reached last way point, set next way point to one before last of array
            // and start to go backwards in the array
            Counter = WayPoints.Count - 2;
            IsGoingBackwards = true;
        }
        if (Counter < 0)
        {
            IsGoingBackwards = false;
            Counter = 1;
        }
        HasUpdatedWayPoint = true;
    }

    public void TakeDamage(int damage)
    {
        stats.TakeDamage(damage);
    }

    private void OnDrawGizmos()
    {
        int i = 0;
        Gizmos.color = Color.yellow;
        for (; i < WayPoints.Count - 1;)
        {
            Gizmos.DrawSphere(WayPoints[i].position, 0.2f);
            Gizmos.DrawLine(WayPoints[i].position, WayPoints[++i].position);
        }
        Gizmos.DrawSphere(WayPoints[i].position, 0.2f);
    }
}
