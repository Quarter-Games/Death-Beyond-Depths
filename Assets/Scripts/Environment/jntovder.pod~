using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public static event Action<CameraController> CameraCreated;
    public static CameraController Instance;
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
    private void Start()
    {
        CameraCreated?.Invoke(this);
    }
    void Update()
    {
        FollowTarget();
    }
    public void FollowTarget()
    {
        var x = Mathf.Min(Mathf.Max(Target.position.x + XOffset * cam.orthographicSize * cam.aspect, Boundries.xMin + (cam.orthographicSize * cam.aspect)), Boundries.xMax - (cam.orthographicSize * cam.aspect));
        var y = Mathf.Min(Mathf.Max(Target.transform.position.y + YOffset * cam.orthographicSize, Boundries.yMin + (cam.orthographicSize)), Boundries.yMax - (cam.orthographicSize));
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
}