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
        if (!collision.TryGetComponent(out CachedPlayerController))
        {
            return;
        }
        if (CachedPlayerController == null) return;
        IsPlayerHiddenInside = true;
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
