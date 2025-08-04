using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static DamageVignetteController;

public class EyeHazardLight : MonoBehaviour
{
    [SerializeField] List<Transform> Waypoints;
    [SerializeField] float TimeToWaitAtWaypoint = 1.5f;
    [SerializeField, Min(0.001f)] float Speed = 5f;
    [SerializeField, Min(0.001f)] float TimeUntilPlayerKill = 2f;
    //[SerializeField] Material LightMaterial;
    [SerializeField] MeshRenderer LightMeshRenderer;
    [SerializeField] Color OriginalLightColor = Color.red;
    [SerializeField] Color AlertLightColor = Color.red;
    
    private int Counter = 0;
    private bool ReachedGoal = false;
    Coroutine WaitCoroutine;
    Vector3 Destination;
    Vector3 StartingPosition;
    float Distance;
    float timer = 0;
    private bool SeesPlayer = false;
    PlayerCharacterController CachedPlayer;
    bool IsInitializing = true;
    static int EyesSeeingPlayer = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out CachedPlayer))
        {
            if (CachedPlayer.IsHidden)
            {
                return;
            }
            EyesSeeingPlayer++;
            LightMeshRenderer.material.DOColor(AlertLightColor, TimeUntilPlayerKill / 2);
            SeesPlayer = true;
            if (EyesSeeingPlayer == 1)
            {
                SpecialEffects.ScreenDamageOverTimeEffect();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out CachedPlayer))
        {
            EyesSeeingPlayer--;
            LightMeshRenderer.material.DOColor(OriginalLightColor, TimeUntilPlayerKill / 4);
            SeesPlayer = false;
            timer = 0;
            if (EyesSeeingPlayer == 0)
            {
                SpecialEffects.ScreenDamageOverTimeEffect();
            }
        }
    }

    private void OnEnable()
    {
        timer = 0;
        LightMeshRenderer.material.color = OriginalLightColor;
        if (!IsInitializing)
        {
            transform.DOMove(StartingPosition, 0.15f).OnComplete(() => MoveToNextWaypoint());
            return;
        }
        IsInitializing = false;
    }
    private void OnDisable()
    {
        LightMeshRenderer.material.color = OriginalLightColor;
        if (WaitCoroutine != null)
        {
            StopCoroutine(WaitCoroutine);
            WaitCoroutine = null;
        }
        DOTween.Kill(transform);
        transform.position = StartingPosition;
    }

    private void Start()
    {
        StartingPosition = transform.position;
        MoveToNextWaypoint();
    }

    private void Update()
    {
        if (SeesPlayer)
        {
            DOTween.Pause(transform);
            if (WaitCoroutine != null)
            {
                StopCoroutine(WaitCoroutine);
                WaitCoroutine = null;
            }
            timer += Time.deltaTime;
            if (timer >= TimeUntilPlayerKill)
            {
                SeesPlayer = false;
                timer = 0;
                if(CachedPlayer) CachedPlayer.TakeDamage(999999);
            }
        }
        else
        {
            timer = 0;
        }
        if (ReachedGoal && WaitCoroutine == null)
        {
            MoveToNextWaypoint();
        }
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
