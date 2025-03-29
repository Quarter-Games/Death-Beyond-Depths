using System;
using System.Collections;
using UnityEngine;

public class ClimablePoint : InteractableObject, IInteractable
{
    public Transform SnippingPoint;
    public ClimablePoint LinkedPoint;
    private void OnValidate()
    {
        if (LinkedPoint != null)
        {
            LinkedPoint.LinkedPoint = this;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (CachedPlayerController != null) return;

        if (collision.TryGetComponent<PlayerCharacterController>(out var player))
        {
            CachedPlayerController = player;
        }
    }
    public void Interact()
    {
        if (CachedPlayerController == null)
        {
            Debug.LogWarning("No Player Controller to interact");
            return;
        }
        CachedPlayerController.StartClimbing(this);
    }
    public void UnInteract()
    {
        Interact();
    }
}
