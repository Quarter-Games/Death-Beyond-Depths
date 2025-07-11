using MoreMountains.Feedbacks;
using UnityEngine;

public class FeelActivator : InteractableObject, IInteractable
{
    [SerializeField] MMF_Player MMF_Player;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
    }
    public void Interact()
    {
        if (MMF_Player) MMF_Player.PlayFeedbacks();
    }

    public void UnInteract()
    {
    }
}
