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
    [SerializeField] float AttackRange;
    [SerializeField] float PatrolSpeed = 0.8f;
    [SerializeField, Min(0)] float WaitingTime;
    [SerializeField] List<Transform> WayPoints;

    private int InitialHP;
    private int Counter;
    private bool ReachedGoal = false;
    private bool ChasingPlayer = false;
    private bool HasUpdatedWayPoint = false;
    private bool IsGoingBackwards = false;
    private float Distance;
    private Vector3 defaultScale;
    private Coroutine PatrolCoroutine;
    private Coroutine PlayerCoroutine;
    private bool IsAttacking;
    private bool hasSpottedPlayerOnce = false;

    public bool IsFacingLeft { get; private set; }
    public bool IsDead => stats.HP <= 0;
    public bool CanPatrol => WayPoints.Count > 0 && !ChasingPlayer && !IsDead;

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
        if (CanPatrol)
            NavAgent.stoppingDistance = 0;
        else
            NavAgent.stoppingDistance = AttackRange;
        defaultScale = transform.localScale;
    }

    private void ReviveAndResetEnemy()
    {
        NavAgent.isStopped = true;
        ChasingPlayer = false;
        hasSpottedPlayerOnce = false;
        StopAllCoroutines();
        StartCoroutine(Revive());
    }

    private IEnumerator Revive()
    {
        yield return new WaitForSeconds(ReviveTimeInSeconds);
        stats.HP = InitialHP;
        gameObject.SetActive(true);
        NavAgent.isStopped = false;
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
        if (IsDead)
            return;
        HandleEnemyFlip();
        if (!hasSpottedPlayerOnce && IsPlayerInVisionCone())
        {
            Debug.Log("Player spotted!");
            if (PlayerCoroutine == null)
                PlayerCoroutine = StartCoroutine(FollowAndAttackPlayer());
        }
        else if (!ChasingPlayer)
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

    private void HandleEnemyFlip()
    {
        if (ChasingPlayer && Player != null)
        {
            Vector3 directionToPlayer = Player.transform.position - transform.position;
            IsFacingLeft = directionToPlayer.x < 0;
        }
        else
        {
            Vector3 velocity = NavAgent.velocity;
            if (velocity.x > 0.1f) // Moving right
            {
                IsFacingLeft = false;
            }
            else if (velocity.x < -0.1f) // Moving left
            {
                IsFacingLeft = true;
            }
        }

        Flip();
        //TODO add flip animations
    }

    protected override void Flip()
    {
        transform.localScale = new Vector3(
            IsFacingLeft ? -Mathf.Abs(defaultScale.x) : Mathf.Abs(defaultScale.x),
            defaultScale.y,
            defaultScale.z
        );
    }

    private bool IsPlayerInVisionCone()
    {
        if (Player == null)
            return false;

        Vector3 directionToPlayer = Player.transform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        if (distanceToPlayer > Radius)
        {
            ChasingPlayer = false;
            return false;
        }
        directionToPlayer.Normalize();
        // If the player is already detected, skip direction checks.
        if (ChasingPlayer)
        {
            NavAgent.stoppingDistance = AttackRange;
            return true;
        }
        else if (Player.IsHidden)
        {
            return false;
        }
        bool isFacingPlayer = !(!IsFacingLeft && directionToPlayer.x < 0) || (IsFacingLeft && directionToPlayer.x > 0);
        if (isFacingPlayer)
        {
            NavAgent.stoppingDistance = AttackRange;
            ChasingPlayer = true;
            HasUpdatedWayPoint = false;
            return true;
        }
        return false;
    }

    private IEnumerator FollowAndAttackPlayer()
    {
        if (PlayerCoroutine != null)
            yield break;

        ChasingPlayer = true;
        if (!hasSpottedPlayerOnce)
        {
            NavAgent.isStopped = true;
            yield return new WaitForSeconds(DelayAfterSpotInSeconds);
            hasSpottedPlayerOnce = true;
            NavAgent.isStopped = false;
        }
        NavAgent.speed = stats.MovementSpeed;

        while (IsPlayerInRoom() && ChasingPlayer)
        {
            if (Player == null) yield break;
            PatrolCoroutine = null;
            NavAgent.SetDestination(Player.transform.position);

            float distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);
            if (distanceToPlayer <= AttackRange && !IsAttacking)
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
            PlayerCoroutine = null;
            //PatrolCoroutine = StartCoroutine(WaitAtPosition());
            Patrol();
        }
    }

    private bool IsPlayerInRoom()
    {
        bool isInRoom = Player != null && Player.CurrentRoom == CurrentRoom;
        if (!isInRoom)
        {
            hasSpottedPlayerOnce = false;
        }
        return isInRoom;
    }

    private void AttackPlayer()
    {
        IsAttacking = true;
        StartCoroutine(WaitForAttackCoolDown(MeleeAttackObject.AttackStats.CooldownTime));
        MeleeAttackObject.gameObject.SetActive(true);
    
    }
    protected IEnumerator WaitForAttackCoolDown(float time)
    {
        yield return new WaitForSeconds(time);
        IsAttacking = false;
        NavAgent.isStopped = false;
    }

    private void Patrol()
    {
        if (WayPoints == null || WayPoints.Count <= 1 || ChasingPlayer)
            return;
        NavAgent.stoppingDistance = 0;
        NavAgent.speed = PatrolSpeed;
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
        Distance = MathF.Abs(transform.position.x - WayPoints[Counter].position.x);
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
        if (WayPoints.Count == 0) return;
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
