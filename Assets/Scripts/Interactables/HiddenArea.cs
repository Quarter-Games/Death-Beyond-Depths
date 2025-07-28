using UnityEngine;

public class HiddenArea : InteractableObject, IInteractable
{
    public bool IsPlayerHiddenInside { get; private set; }

    public bool Interact()
    {
        Debug.Log("Hide");
        return true;
    }

    public void UnInteract()
    {

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out CachedPlayerController))
        {
            return;
        }
        if (CachedPlayerController == null) return;
        IsPlayerHiddenInside = true;
        CachedPlayerController.CanHide = true;
        if (!CachedPlayerController.IsStanding)
        {
            CachedPlayerController.Hide();
        }
        //else
        //{
        //    CachedPlayerController.CanCrouch = false;
        //}
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out CachedPlayerController))
        {
            return;
        }
        if (CachedPlayerController == null) return;
        CachedPlayerController.CanHide = false;
        UnHidePlayer();
        CachedPlayerController.CanCrouch = true;
    }

    public void UnHidePlayer()
    {
        IsPlayerHiddenInside = false;
        if (CachedPlayerController == null) return;
        if (CachedPlayerController.IsHidden)
        {
            CachedPlayerController.StopHiding();
        }
    }
}
