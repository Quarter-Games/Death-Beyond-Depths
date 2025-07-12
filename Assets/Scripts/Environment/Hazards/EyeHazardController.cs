using DG.Tweening;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class EyeHazardController : MonoBehaviour
{
    [SerializeField] bool IsEyeOpenClose = true;
    [SerializeField] float TimeToOpen = 1f;
    [SerializeField] float TimeToClose = 1f;

    [Space(10)]
    [SerializeField] bool IsLightMoving = false;
    [SerializeField] GameObject LightObject;
    [SerializeField] GameObject FollowObject;
    [SerializeField] GameObject PupilObject;
    [SerializeField] MMF_Player MMFOpenEye;
    [SerializeField] MMF_Player MMFCloseEye;
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

    private void Start()
    {
        IsEyeOpenClose = false;
        IsEyeOpened = false;
        EyeClose();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsEyeOpenClose)
        {
            return;
        }
        timer += Time.deltaTime;
        HandleEyeState();
        if (!IsLightMoving)
        {
            return;
        }
        LightObject.transform.LookAt(FollowObject.transform, Vector3.forward);
    }

    public void EnableEyeHazard()
    {
        IsEyeOpenClose = true;
        StartCoroutine(EyeOpen());
    }

    private void HandleEyeState()
    {
        if (IsEyeOpened)
        {
            if (timer < TimeToClose)
            {
                return;
            }
            StartCoroutine(EyeClose());
            return;
        }
        if (timer < TimeToOpen)
        {
            return;
        }
        StartCoroutine(EyeOpen());
    }

    IEnumerator EyeOpen()
    {
        timer = 0;
        yield return MMFOpenEye.PlayFeedbacksCoroutine(Vector3.one);
        timer = 0;
        IsEyeOpened = true;
    }

    IEnumerator EyeClose()
    {
        timer = 0;
        yield return MMFCloseEye.PlayFeedbacksCoroutine(Vector3.one);
        timer = 0;
        IsEyeOpened = false;
    }
}
