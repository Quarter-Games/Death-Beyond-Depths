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
    [SerializeField] float TurnAroundDelaySeconds = 0.5f;
    [SerializeField] float TurnAroundSpeed;

    Coroutine TurningCoroutine;

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
    }

    void Update()
    {
        //HandleCameraBoundries();
        FollowTarget();
    }

    public void FollowTarget()
    {
        var x = Mathf.Min(Mathf.Max(Target.position.x + XOffset * cam.orthographicSize * cam.aspect, Boundries.xMin + (cam.orthographicSize * cam.aspect)), Boundries.xMax - (cam.orthographicSize * cam.aspect));
        var y = Mathf.Min(Mathf.Max(Target.transform.position.y + YOffset * cam.orthographicSize, Boundries.yMin + (cam.orthographicSize)), Boundries.yMax - (cam.orthographicSize));
        //transform.position = Vector3.Lerp(transform.position, FinalTargetPosition, CameraSpeed);
        //Vector3.Lerp(transform.position, new Vector3(x, y, transform.position.z), CameraSpeed);
        transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, transform.position.z), CameraSpeed);

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
        yield return new WaitForSeconds(TurnAroundDelaySeconds);
        //XOffset *= -1;
        //if (isFacingRight )
        //{
        //    XOffset = MathF.Abs(XOffset);
        //}
        //TurningCoroutine = null;
        float targetXOffset = isFacingRight ? MathF.Abs(XOffset) : -MathF.Abs(XOffset);

        DOTween.To(
            () => XOffset,
            value => XOffset = value,
            targetXOffset,
            TurnAroundSpeed
        ).SetEase(Ease.InOutQuint);//.SetEase(Ease.OutQuint);

        TurningCoroutine = null;
    }

    private void HandleCameraBoundries()
    {
        RaycastHit2D Right = Physics2D.Raycast(Target.position, Vector2.right, horizontalDistance, TargerLayer.value);
        RaycastHit2D Left = Physics2D.Raycast(Target.position, Vector2.left, horizontalDistance, TargerLayer.value);
        RaycastHit2D Up = Physics2D.Raycast(Target.position, Vector2.up, verticalDistance, TargerLayer.value);
        RaycastHit2D Down = Physics2D.Raycast(Target.position, Vector2.down, verticalDistance, TargerLayer.value);

        if (Right.collider != null && Left.collider != null)
        {
            FinalTargetPosition.x = (Right.point.x + Left.point.x) / 2;
        }
        else if (Right.collider != null)
        {
            FinalTargetPosition.x = Right.point.x - horizontalDistance;
        }
        else if (Left.collider != null)
        {
            FinalTargetPosition.x = Left.point.x + horizontalDistance;
        }

        if (Up.collider != null && Down.collider != null)
        {
            FinalTargetPosition.y = (Up.point.y + Down.point.y) / 2;
        }
        else if (Up.collider != null)
        {
            FinalTargetPosition.y = Up.point.y - verticalDistance;
        }
        else if (Down.collider != null)
        {
            FinalTargetPosition.y = Down.point.y + verticalDistance;
        }
    }
}