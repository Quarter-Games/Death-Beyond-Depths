using DG.Tweening;
using System;
using UnityEditor;
using UnityEngine;

public class EyeHazardController : MonoBehaviour
{
    [SerializeField] bool IsEyeOpenClose = true;
    [SerializeField] float TimeToOpen = 1f;
    [SerializeField] float TimeToClose = 1f;
    [SerializeField] float TimeStaysOpen = 1f;
    [SerializeField] float TimeStaysClosed = 2f;

    [Space(10)]
    [SerializeField] bool IsLightMoving = false;
    [SerializeField] GameObject LightObject;
    [SerializeField] GameObject FollowObject;

    bool IsEyeOpened = true;
    Vector3 StartingPosition;
    float timer = 0;

    private void OnValidate()
    {
        if (LightObject == null)
        {
            LightObject = gameObject.GetComponentInChildren<Light>().gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (IsEyeOpenClose)
        {
            HandleEyeState();
        }
        if (!IsLightMoving)
        {
            return;
        }
        LightObject.transform.LookAt(FollowObject.transform, Vector3.forward);
    }

    private void HandleEyeState()
    {
        if (IsEyeOpened)
        {
            if (timer < TimeToClose)
            {
                return;
            }
            EyeClose();
            return;
        }
        if (timer < TimeToOpen)
        {
            return;
        }
        EyeOpen();
    }

    void EyeOpen()
    {
        FollowObject.transform.position = StartingPosition;
        LightObject.SetActive(true);
        FollowObject.SetActive(true);
        IsEyeOpened = true;
        timer = 0;
    }

    void EyeClose()
    {
        LightObject.SetActive(false);
        FollowObject.SetActive(false);
        IsEyeOpened = false;
        timer = 0;
    }
}
