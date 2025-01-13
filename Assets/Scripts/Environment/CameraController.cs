using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem.XR;

public class CameraController : MonoBehaviour
{
    [SerializeField] public Rect Boundries;
    [SerializeField] Camera cam;
    [SerializeField]
    [Range(-1f, 1f)] float XOffset;
    [SerializeField]
    [Range(-1f, 1f)] float YOffset;

    [Tooltip("How fast does camera follows the player \n 0 - don't, 1 - immediatlly")]
    [Range(0, 1)]
    [SerializeField] float CameraSpeed;
    [SerializeField] LayerMask DefaultLayer;
    [SerializeField] LayerMask InvisibilityLayer;

    [SerializeField]
    [Tooltip("This is a size of camera for screen with 1:1 ratio")]
    [Min(0)] float Size = 10;
    public Transform Target;
    [SerializeField] Vector3 FinalTargetPosition;
    [SerializeField] private LayerMask TargerLayer;
    [SerializeField] private float horizontalDistance = 1;
    [SerializeField] private float verticalDistance = 1;

    public static event Action<CameraController> CameraCreated;
    public static CameraController Instance;

    [Header("Camera Turn Around")]
    [SerializeField] float PlayerLockSeconds = 0.05f;
    [SerializeField] float TurnAroundDelaySeconds = 0.5f;
    [SerializeField] float TurnAroundSpeed;

    [SerializeField] private float BorderPaddingY;
    [SerializeField] private float BorderPaddingX;
    bool IsXLocked = false;
    bool IsYLocked = false;
    Collider2D ActiveXBorder;
    Collider2D ActiveYBorder;
    Collider2D CameraCollider;
    float LockedXPosition;
    float LockedYPosition;
    
    Coroutine TurningCoroutine;

    private const string CAMERA_BOUNDARY_TAG = "CameraBoundary";

    private void OnValidate()
    {
        cam.orthographicSize = Size / cam.aspect;
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        cam.orthographicSize = Size / cam.aspect;
    }

    private void OnEnable()
    {
        PlayerCharacterController.OnFlip += CameraTurnAround;
    }

    private void OnDisable()
    {
        PlayerCharacterController.OnFlip -= CameraTurnAround;
    }

    private void Start()
    {
        CameraCreated?.Invoke(this);
        FinalTargetPosition.z = transform.position.z;
        CameraCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        FollowTarget();
    }

    public void FollowTarget()
    {
        Vector3 targetPosition = transform.position;
        if(IsXLocked)
        {
            targetPosition.x = LockedXPosition;
        } 
        if(IsYLocked)
        {
            targetPosition.y = LockedYPosition;
        }
        var x = Mathf.Min(Mathf.Max(Target.position.x + XOffset * cam.orthographicSize * cam.aspect, Boundries.xMin + (cam.orthographicSize * cam.aspect)), Boundries.xMax - (cam.orthographicSize * cam.aspect));
        var y = Mathf.Min(Mathf.Max(Target.position.y + YOffset * cam.orthographicSize, Boundries.yMin + (cam.orthographicSize)), Boundries.yMax - (cam.orthographicSize));
        transform.position = Vector3.Lerp(targetPosition, new Vector3(x, y, transform.position.z), CameraSpeed);

    }
    public void ActivateInvisibilityLayer()
    {
        cam.cullingMask = InvisibilityLayer;
    }
    public void DeactivateInvisibilityLayer()
    {
        cam.cullingMask = DefaultLayer;
    }

    private void CameraTurnAround(bool isFacingRight)
    {
        if (TurningCoroutine != null) return;
        TurningCoroutine = StartCoroutine(CameraTurnCoroutine(isFacingRight));
    }
    private IEnumerator CameraTurnCoroutine(bool isFacingRight)
    {
        StartCoroutine(PlayerCharacterController.DisableThenEnableInputSeconds(PlayerLockSeconds));
        yield return new WaitForSeconds(TurnAroundDelaySeconds);
        float targetXOffset = isFacingRight ? MathF.Abs(XOffset) : -MathF.Abs(XOffset);

        DOTween.To(
            () => XOffset,
            value => XOffset = value,
            targetXOffset,
            TurnAroundSpeed
        ).SetEase(Ease.InOutQuint);//.SetEase(Ease.OutQuint);

        TurningCoroutine = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(CAMERA_BOUNDARY_TAG)) return;
        if(!IsXLocked &&
            (MathF.Abs(collision.bounds.min.x - CameraCollider.bounds.min.x) < BorderPaddingX) ||
            MathF.Abs(collision.bounds.max.x - CameraCollider.bounds.max.x) < BorderPaddingX)
        {
            IsXLocked = true;
            LockedXPosition = transform.position.x;
            ActiveXBorder = collision;
        }
        if (!IsYLocked &&
            (MathF.Abs(collision.bounds.min.y - CameraCollider.bounds.min.y) < BorderPaddingY) ||
            MathF.Abs(collision.bounds.max.y - CameraCollider.bounds.max.y) < BorderPaddingY)
        {
            IsYLocked = true;
            LockedYPosition = transform.position.y;
            ActiveYBorder = collision;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(CAMERA_BOUNDARY_TAG)) return;
        if(IsXLocked && ActiveXBorder == collision)
        {
            IsXLocked = false;
            ActiveXBorder = null;
        }

        if (IsYLocked && ActiveYBorder == collision)
        {
            IsYLocked = false;
            ActiveYBorder = null;
        }
    }
}