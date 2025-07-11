using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

internal class HallwayPuzzleManager : InteractableObject, IInteractable
{
    [SerializeField] private PuzzleState _puzzleState;
    [SerializeField] GameObject RoomPrefab;
    [SerializeField] GameObject OriginalRoomInstance;
    [SerializeField] GameObject RoomCopy;
    [SerializeField] BoxCollider2D cameraBoundries;
    [SerializeField] BoxCollider2D CopyBoundries;
    [SerializeField] float RoomWidth;
    private CinemachineConfiner2D _confiner2D;

    private Vector3 _originalRoomPosition;
    private Vector3 _originalColliderPosition;
    override protected void OnEnable()
    {
        base.OnEnable();
        HallwayPuzzleSideTrigger.OnSideTriggerEntered += OnSideReached;
    }
    override protected void OnDisable()
    {
        base.OnDisable();
        HallwayPuzzleSideTrigger.OnSideTriggerEntered -= OnSideReached;
    }

    private void OnSideReached(HallwayPuzzleSideTrigger trigger)
    {
        if (!RoomCopy) return;
        if (trigger.isRight)
        {
            Debug.Log("PUZZLE ENTERED");
            //Copy is on the left
            if (RoomCopy.transform.position.x < OriginalRoomInstance.transform.position.x)
            {
                Debug.Log("PUZZLE ENTERED - Copy is on the left");
                if (trigger.transform.position.x > OriginalRoomInstance.transform.position.x)
                {
                    Debug.Log("Player is on the right side of the original room");
                    //Player is on the right side of the original room
                    RoomCopy.transform.position = new Vector3(RoomCopy.transform.position.x + RoomWidth, RoomCopy.transform.position.y, RoomCopy.transform.position.z);
                    CopyBoundries.transform.position = new Vector3(CopyBoundries.transform.position.x + RoomWidth, CopyBoundries.transform.position.y, CopyBoundries.transform.position.z);
                    StartCoroutine(ForceCinemachineUpdate());
                }
            }
            else
            {
                Debug.Log("PUZZLE ENTERED - Copy is on the right");
                if (trigger.transform.position.x > RoomCopy.transform.position.x)
                {
                    Debug.Log("Player is on the right side of the copy room");
                    //Player is on the right side of the copy room
                    OriginalRoomInstance.transform.position = new Vector3(OriginalRoomInstance.transform.position.x + RoomWidth, OriginalRoomInstance.transform.position.y, OriginalRoomInstance.transform.position.z);
                    cameraBoundries.transform.position = new Vector3(cameraBoundries.transform.position.x + RoomWidth, cameraBoundries.transform.position.y, cameraBoundries.transform.position.z);
                }
                StartCoroutine(ForceCinemachineUpdate());
            }
            _confiner2D.InvalidateBoundingShapeCache();
        }
        else
        {
            Debug.Log("PUZZLE ENTERED - Left side");
            if (trigger.transform.position.x < OriginalRoomInstance.transform.position.x && trigger.transform.position.x < RoomCopy.transform.position.x)
            {
                Debug.Log("PUZZZLE ENDED");
                Vector3 delta;
                if (RoomCopy.transform.position.x < OriginalRoomInstance.transform.position.x)
                {
                    delta = RoomCopy.transform.position - _originalRoomPosition;
                    RoomCopy.transform.position = _originalRoomPosition;
                    CopyBoundries.transform.position = _originalColliderPosition;

                }
                else
                {
                    delta = OriginalRoomInstance.transform.position - _originalRoomPosition;
                    OriginalRoomInstance.transform.position = _originalRoomPosition;
                    cameraBoundries.transform.position = _originalColliderPosition;
                }
                var player = InventoryManager.Instance.Player;
                var camera = CameraManager.Instance;
                var vcam = camera.VirtualCameras[0];
                var followObject = vcam.Follow;

                player.transform.position -= delta;
                followObject.transform.position -= delta;

                _confiner2D.InvalidateBoundingShapeCache();

                vcam.OnTargetObjectWarped(followObject.transform, -delta);

                StartCoroutine(ForceCinemachineUpdate());
                gameObject.SetActive(false);
            }
        }
        StartCoroutine(ForceCinemachineUpdate());

    }

    protected override void OnTriggerEnter2D(Collider2D collision) { }
    protected override void OnTriggerExit2D(Collider2D collision) { }
    public void Interact()
    {
        switch (_puzzleState)
        {
            case PuzzleState.BeforeStart:
                StartPuzzle();
                break;
        }
    }

    private void StartPuzzle()
    {
        _originalRoomPosition = OriginalRoomInstance.transform.position;
        OriginalRoomInstance.transform.position = new Vector3(_originalRoomPosition.x, 1000, _originalRoomPosition.z);
        RoomCopy = Instantiate(RoomPrefab, OriginalRoomInstance.transform.position + new Vector3(RoomWidth, 0, 0), Quaternion.identity);

        var delta = OriginalRoomInstance.transform.position - _originalRoomPosition;

        var player = InventoryManager.Instance.Player;
        var camera = CameraManager.Instance;
        var vcam = camera.VirtualCameras[0];
        var followObject = vcam.Follow;

        // Move targets
        _originalColliderPosition = cameraBoundries.transform.position;
        player.transform.position += delta;
        followObject.transform.position += delta;
        cameraBoundries.transform.position += delta;
        CopyBoundries = Instantiate(cameraBoundries, cameraBoundries.transform.position + new Vector3(RoomWidth, 0, 0), Quaternion.identity, cameraBoundries.transform.parent);

        // Regenerate collider geometry if needed
        if (cameraBoundries.TryGetComponent<CompositeCollider2D>(out var composite))
        {
            composite.GenerateGeometry();
        }

        // Invalidate confiner cache
        _confiner2D = vcam.GetComponent<CinemachineConfiner2D>();
        _confiner2D.InvalidateBoundingShapeCache();

        // Notify Cinemachine about target warp
        vcam.OnTargetObjectWarped(followObject.transform, delta);

        // Let Cinemachine catch up next frame
        StartCoroutine(ForceCinemachineUpdate());
    }

    private IEnumerator ForceCinemachineUpdate()
    {

        var camera = CameraManager.Instance;
        var vcam = camera.VirtualCameras[0];
        var follow = vcam.Follow;

        vcam.OnTargetObjectWarped(follow.transform, Vector3.zero);
        yield return new WaitForEndOfFrame();
        _confiner2D.InvalidateBoundingShapeCache();

    }

    public void UnInteract()
    {

    }
    public enum PuzzleState
    {
        BeforeStart,
        PuzzleEntered,
        PuzzleCompleted
    }
}
