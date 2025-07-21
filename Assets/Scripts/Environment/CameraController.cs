using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [SerializeField] Rigidbody2D CamRB;
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
    [SerializeField] private LayerMask CameraBlockLayer;
    [SerializeField] private float horizontalDistance = 1;
    [SerializeField] private float verticalDistance = 1;

    public static event Action<CameraController> CameraCreated;
    public static CameraController Instance;

    [Header("Camera Turn Around")]
    [SerializeField] float PlayerLockSeconds = 0.05f;
    [SerializeField] float TurnAroundDelaySeconds = 0.5f;
    [SerializeField] float TurnAroundSpeed;

    [Header("Camera Shake")]
    [SerializeField] AnimationCurve Curve;
    [SerializeField] float Duration = 0.5f;

    [Space(10)]
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
    Coroutine ShakeCoroutine;

    private const string CAMERA_BOUNDARY_TAG = "CameraBoundary";

    private void OnValidate()
    {
        cam.orthographicSize = Size / cam.aspect;
        CamRB = GetComponent<Rigidbody2D>();
    }
    private void Awake()
    {
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

    //void Update()
    //{
    //    FollowTarget();
    //}

    private void FixedUpdate()
    {
        FollowTarget();
    }

    public void FollowTarget()
    {
        if (ShakeCoroutine != null) return;

        Vector2 targetPosition = transform.position;

        // If locked, maintain the locked position
        if (IsXLocked)
        {
            targetPosition.x = LockedXPosition;
        }
        if (IsYLocked)
        {
            targetPosition.y = LockedYPosition;
        }

        Vector2 movementDirection = targetPosition - CamRB.position;
        float movementDistance = movementDirection.magnitude;

        // Cast the entire collider instead of using a single ray
        RaycastHit2D[] hits = new RaycastHit2D[1]; // Store hit results
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(CameraBlockLayer);
        filter.useTriggers = false; // Ignore triggers

        int hitCount = CameraCollider.Cast(movementDirection.normalized, filter, hits, movementDistance);

        if (hitCount > 0)
        {
            RaycastHit2D hit = hits[0];

            // If the player has moved beyond the blocking object, allow movement
            if (Vector2.Dot(movementDirection, (Target.position - new Vector3(hit.point.x, hit.point.y, Target.position.z)).normalized) > 0)
            {
                targetPosition = Target.position;
            }
            else
            {
                targetPosition = CamRB.position; // Stop movement if still behind obstacle
            }
        }

        // Calculate clamped position based on boundaries
        float x = Mathf.Clamp(Target.position.x + XOffset * cam.orthographicSize * cam.aspect,
                              Boundries.xMin + (cam.orthographicSize * cam.aspect),
                              Boundries.xMax - (cam.orthographicSize * cam.aspect));

        float y = Mathf.Clamp(Target.position.y + YOffset * cam.orthographicSize,
                              Boundries.yMin + cam.orthographicSize,
                              Boundries.yMax - cam.orthographicSize);

        Vector2 finalPosition = Vector2.Lerp(targetPosition, new Vector2(x, y), Time.fixedDeltaTime * CameraSpeed);
        CamRB.MovePosition(finalPosition);
    }


    public void ActivateInvisibilityLayer()
    {
        cam.cullingMask = InvisibilityLayer;
    }
    public void DeactivateInvisibilityLayer()
    {
        cam.cullingMask = DefaultLayer;
    }

    public void ShakeCamera()
    {
        if (ShakeCoroutine != null) return;
        ShakeCoroutine = StartCoroutine(ShakeCameraCoroutine());
    }

    private IEnumerator ShakeCameraCoroutine()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = Curve.Evaluate(elapsedTime / Duration);
            transform.position = startPosition + UnityEngine.Random.insideUnitSphere;
            yield return null;
        }
        transform.position = startPosition;
        ShakeCoroutine = null;
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
        ).SetEase(Ease.InOutQuint);

        TurningCoroutine = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(CAMERA_BOUNDARY_TAG)) return;
        if (!IsXLocked &&
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
        if (IsXLocked && ActiveXBorder == collision)
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