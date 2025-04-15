using System.Collections;
using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;

public class CameraFollowObject : MonoBehaviour
{
    [SerializeField] PlayerCharacterController Player;
    [SerializeField] Collider2D ConfinerBounds;
    [SerializeField] float FlipDuration = 0.5f;

    Coroutine TurnCoroutine;

    private void OnEnable()
    {
        PlayerCharacterController.OnFlip += CallTurn;
    }

    private void OnDisable()
    {
        PlayerCharacterController.OnFlip -= CallTurn;
    }

    private void Update()
    {
        //if(!ConfinerBounds.OverlapPoint(Player.transform.position))
        //{
        //    return;
        //}
        transform.position = Player.transform.position;
    }

    private void CallTurn(bool isFacingRight)
    {
        if (TurnCoroutine == null)
        {
            TurnCoroutine = StartCoroutine(FlipOnYAxis(isFacingRight));
        }
    }

    public IEnumerator FlipOnYAxis(bool isFacingRight)
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotation = isFacingRight ? 180f : 0;
        float yRotation = 0;
        float elapsedTime = 0;
        while (elapsedTime < FlipDuration)
        {
            elapsedTime += Time.deltaTime;
            DOTween.To(() => yRotation, y => yRotation = y, endRotation, (elapsedTime / FlipDuration)).SetEase(Ease.InOutSine);
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            yield return null;
        }
    }
}
