using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    [SerializeField] List<CinemachineCamera> VirtualCameras;

    CinemachineCamera CurrentCamera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < VirtualCameras.Count; i++)
        {
            if (VirtualCameras[i].gameObject.activeSelf)
            {
                CurrentCamera = VirtualCameras[i];
            }
        }
    }
    public void IgnoreBounderies(bool isIgnore)
    {
        if (CurrentCamera.TryGetComponent(out CinemachineConfiner2D confiner))
        {
            confiner.enabled = !isIgnore;
        }
    }

    public void SwapCamera(CinemachineCamera cameraFromLeft, CinemachineCamera cameraFromRight, CinemachineCamera cameraFromUp, CinemachineCamera cameraFromDown, Vector2 triggerExitDirection)
    {
        Debug.Log("Swapping from camera " + CurrentCamera);
        if (cameraFromLeft != null && CurrentCamera == cameraFromLeft && triggerExitDirection.x > 0f)
        {
            cameraFromRight.gameObject.SetActive(true);
            cameraFromLeft.gameObject.SetActive(false);
            CurrentCamera = cameraFromRight;
            Debug.Log(" To " + CurrentCamera);
            return;
        }
        else if (cameraFromRight != null && CurrentCamera == cameraFromRight && triggerExitDirection.x <= 0f)
        {
            cameraFromLeft.gameObject.SetActive(true);
            cameraFromRight.gameObject.SetActive(false);
            CurrentCamera = cameraFromLeft;
            Debug.Log(" To " + CurrentCamera);
            return;
        }
        else if (cameraFromUp != null && CurrentCamera == cameraFromUp && triggerExitDirection.y > 0f)
        {
            cameraFromDown.gameObject.SetActive(true);
            cameraFromUp.gameObject.SetActive(false);
            CurrentCamera = cameraFromDown;
            Debug.Log(" To " + CurrentCamera);
            return;
        }
        else if (cameraFromDown != null && CurrentCamera == cameraFromDown && triggerExitDirection.y <= 0f)
        {
            cameraFromUp.gameObject.SetActive(true);
            cameraFromDown.gameObject.SetActive(false);
            CurrentCamera = cameraFromUp;
            Debug.Log(" To " + CurrentCamera);
        }
    }
}
