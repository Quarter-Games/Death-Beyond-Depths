using MoreMountains.Feedbacks;
using System;
using UnityEngine;

public class ClocheInteractable : InteractableObject, IInteractable
{
    [SerializeField] MMF_Player MMF_Player;
    public bool IsInteractable = false;
    [SerializeField] GameObject RewardPickUp;

    public static event Action<ClocheInteractable> OnClocheInteracted;
    protected override void ActivateInteractionUI(Collider2D collision)
    {
        if (!IsInteractable) return;
        base.ActivateInteractionUI(collision);
    }
    protected override void DeactivateInteractionUI(Collider2D collision)
    {
        if (!IsInteractable) return;
        base.DeactivateInteractionUI(collision);
    }
    public void EnableClocheInteractability()
    {
        IsInteractable = true;
    }

    public void DisableClocheInteractability()
    {
        UITrigger.gameObject.SetActive(false);
        InteractionTrigger.gameObject.SetActive(false);
        IsInteractable = false;
        IndicatorUI.SetActive(false);
        InformManagerOfNonInteractability();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsInteractable) return;
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }

    public bool Interact()
    {
        if (!IsInteractable) return false;
        Debug.Log("Cloche Interacted");
        MMF_Player.PlayFeedbacks();
        DisableClocheInteractability();
        OnClocheInteracted?.Invoke(this);
        if (RewardPickUp) RewardPickUp.SetActive(true);
        return true;
    }

    public void UnInteract()
    {

    }
}
