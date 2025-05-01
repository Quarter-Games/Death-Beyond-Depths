using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

internal class CameraFocusMovement : InteractableObject, IInteractable
{
    public List<Transform> TargetPosition;
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
            CameraManager.Instance.IgnoreBounderies(true);
            yield return new WaitForSeconds(MoveDelay);
            int index = 0;
            MoveToNextPosition();
            void MoveToNextPosition()
            {
                if (index >= TargetPosition.Count)
                {
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
                    return;
                }
                CameraFollowObject.Instance.transform.DOMove(TargetPosition[index].position, MoveDuration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    index++;
                    MoveToNextPosition();
                });
            }

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