using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EyeHazardLight : MonoBehaviour
{
    [SerializeField] List<Transform> Waypoints;
    [SerializeField] float TimeToWaitAtWaypoint = 1.5f;
    [SerializeField, Min(0.001f)] float Speed = 5f;

    private int Counter = 0;
    private bool ReachedGoal = false;
    Coroutine WaitCoroutine;
    Vector3 Destination;
    Vector3 StartingPosition;
    float Distance;

    bool IsInitializing = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerCharacterController player))
        {
            if (player.IsHidden)
            {
                return;
            }
            player.TakeDamage(999999);
        }
    }

    private void OnEnable()
    {
        if (!IsInitializing)
        {
            transform.position = StartingPosition;
            MoveToNextWaypoint();
            return;
        }
        IsInitializing = false;
    }
    private void OnDisable()
    {
        if (WaitCoroutine != null)
        {
            StopCoroutine(WaitCoroutine);
            WaitCoroutine = null;
        }
        DOTween.Kill(transform);
    }

    private void Start()
    {
        StartingPosition = transform.position;
        MoveToNextWaypoint();
    }

    private void MoveToNextWaypoint()
    {
        if (Waypoints == null || Waypoints.Count == 0) return;

        Destination = Waypoints[Counter].position;
        ReachedGoal = false;
        transform.DOMove(Destination, Speed)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => WaitCoroutine = StartCoroutine(WaitAtWayPoint()));
    }

    private IEnumerator WaitAtWayPoint()
    {
        ReachedGoal = true;
        yield return new WaitForSeconds(TimeToWaitAtWaypoint);

        Counter = (Counter + 1) % Waypoints.Count; // Loop to next waypoint
        MoveToNextWaypoint();
    }
}
