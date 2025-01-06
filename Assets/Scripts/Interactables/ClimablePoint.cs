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
