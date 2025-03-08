using UnityEngine;

public class HiddenArea : InteractableObject, IInteractable
{
    public bool IsPlayerHiddenInside { get; private set; }

    public void Interact()
    {
        Debug.Log("Hide");
    }

    public void UnInteract()
    {

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (CachedPlayerController == null) return;
        IsPlayerHiddenInside = true;
        if (!CachedPlayerController.IsStanding)
        {
            CachedPlayerController.Hide();
            Debug.Log("hiding player");
        }
        else
        {
            CachedPlayerController.CanCrouch = false;
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (CachedPlayerController == null) return;
        UnHidePlayer();
        CachedPlayerController.CanCrouch = true;
    }

    public void UnHidePlayer()
    {
        IsPlayerHiddenInside = false;
        if (CachedPlayerController.IsHidden)
        {
            CachedPlayerController.StopHiding();
        }
    }
}
