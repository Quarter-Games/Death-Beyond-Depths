using UnityEngine;

public class HiddenArea : InteratableObject, IInteractable
{
    void IInteractable.Interact()
    {
        Debug.Log("Hide");
    }

    void IInteractable.UnInteract()
    {
        
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (CachedPlayerController.IsStanding)
        {
            CachedPlayerController.Hide();
        }
        else
        {
            CachedPlayerController.CanCrouch = false;
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (CachedPlayerController.IsHidden)
        {
            CachedPlayerController.StopHiding();
        }
        CachedPlayerController.CanCrouch = true;
    }
}
