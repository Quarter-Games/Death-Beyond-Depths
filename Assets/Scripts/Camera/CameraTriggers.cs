using System;
using Unity.Cinemachine;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using NUnit.Framework;
using System.Collections.Generic;

public class CameraTriggers : MonoBehaviour
{
    public CustomInspectorCameras CustomInspectorCameras;
    [SerializeField] Collider2D Collider;

    PlayerCharacterController CachedPlayerController;


    private void Start()
    {
        CustomInspectorCameras.Cameras.Add(CustomInspectorCameras.CameraOnLeft);
        CustomInspectorCameras.Cameras.Add(CustomInspectorCameras.CameraOnRight);
        CustomInspectorCameras.Cameras.Add(CustomInspectorCameras.CameraOnUp);
        CustomInspectorCameras.Cameras.Add(CustomInspectorCameras.CameraOnDown);
    }

    private int NumberOfValidCameras()
    {
        int num = 0;
        foreach (var camera in CustomInspectorCameras.Cameras)
        {
            if(camera == null) continue;
            num++;
        }
        return num;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out CachedPlayerController))
        {
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out CachedPlayerController))
        {
            return;
        }
        Vector2 exitDirection = (CachedPlayerController.transform.position - Collider.bounds.center).normalized;
        if(CustomInspectorCameras.SwapCameras && NumberOfValidCameras() >= 2)
        {
            CameraManager.Instance.SwapCamera(CustomInspectorCameras.CameraOnLeft, CustomInspectorCameras.CameraOnRight, CustomInspectorCameras.CameraOnUp, CustomInspectorCameras.CameraOnDown, exitDirection);
        }
    }
}

[Serializable]
public class CustomInspectorCameras
{
    public bool SwapCameras;

    [HideInInspector] public CinemachineCamera CameraOnLeft;
    [HideInInspector] public CinemachineCamera CameraOnRight;
    [HideInInspector] public CinemachineCamera CameraOnUp;
    [HideInInspector] public CinemachineCamera CameraOnDown;
    [HideInInspector] public List<CinemachineCamera> Cameras;
}

#if UNITY_EDITOR
[CustomEditor(typeof(CameraTriggers))]
public class CameraEditor : Editor
{
    CameraTriggers cameraTriggers;
    private void OnEnable()
    {
        cameraTriggers = (CameraTriggers)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(cameraTriggers.CustomInspectorCameras.SwapCameras)
        {
            cameraTriggers.CustomInspectorCameras.CameraOnLeft = EditorGUILayout.ObjectField("Camera On Left", 
                cameraTriggers.CustomInspectorCameras.CameraOnLeft, typeof(CinemachineCamera), true) as CinemachineCamera;
            cameraTriggers.CustomInspectorCameras.CameraOnRight = EditorGUILayout.ObjectField("Camera On Right", 
                cameraTriggers.CustomInspectorCameras.CameraOnRight, typeof(CinemachineCamera), true) as CinemachineCamera;
            cameraTriggers.CustomInspectorCameras.CameraOnUp = EditorGUILayout.ObjectField("Camera On Up",
                cameraTriggers.CustomInspectorCameras.CameraOnUp, typeof(CinemachineCamera), true) as CinemachineCamera;
            cameraTriggers.CustomInspectorCameras.CameraOnDown = EditorGUILayout.ObjectField("Camera On Down",
                cameraTriggers.CustomInspectorCameras.CameraOnDown, typeof(CinemachineCamera), true) as CinemachineCamera;
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(cameraTriggers);
        }
    }
}
#endif