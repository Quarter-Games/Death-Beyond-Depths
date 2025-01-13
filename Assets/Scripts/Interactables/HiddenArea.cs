using UnityEngine;

public class HiddenArea : InteractableObject, IInteractable
{
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
        if (!CachedPlayerController.IsStanding)
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
        if (CachedPlayerController == null) return;
        if (CachedPlayerController.IsHidden)
        {
            CachedPlayerController.StopHiding();
        }
        CachedPlayerController.CanCrouch = true;
    }
}
