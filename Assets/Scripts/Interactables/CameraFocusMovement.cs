using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

internal class CameraFocusMovement : InteractableObject, IInteractable
{
    public Transform TargetPosition;
    public float MoveDelay = 0.5f;
    public float MoveDuration = 1f;
    public float StandbyDuration = 1f;
    public void Interact()
    {
        CameraFollowObject.Instance.StartCoroutine(MoveCamera());
        IEnumerator MoveCamera()
        {
            var actions = InputSystem.ListEnabledActions();
            InputSystem.DisableAllEnabledActions();
            CameraFollowObject.Instance.isMoving = false;
            var startPos = CameraFollowObject.Instance.transform.position;
            yield return new WaitForSeconds(MoveDelay);
            CameraFollowObject.Instance.transform.DOMove(TargetPosition.position, MoveDuration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    CameraManager.Instance.IgnoreBounderies(true);
                    CameraFollowObject.Instance.StartCoroutine(StandBy());
                    IEnumerator StandBy()
                    {
                        yield return new WaitForSeconds(StandbyDuration);
                        CameraFollowObject.Instance.transform.DOMove(startPos, MoveDuration)
                            .SetEase(Ease.InOutSine).OnComplete(() =>
                            {
                                CameraManager.Instance.IgnoreBounderies(false);
                                CameraFollowObject.Instance.isMoving = true;
                                actions.ForEach(x => x.Enable());
                            });
                    }
                });

        }
    }

    public void UnInteract()
    {
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
    }

}