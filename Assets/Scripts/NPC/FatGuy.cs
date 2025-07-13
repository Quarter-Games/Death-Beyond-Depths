using MoreMountains.Feedbacks;
using UnityEngine;

public class FatGuy : InteractableObject, IInteractable
{
    [SerializeField] InventoryItem itemToGive;
    [SerializeField] MMF_Player Desintegrate;
    [SerializeField] Collider2D BlockerCollider;
    protected override void ActivateInteractionUI(Collider2D collision)
    {
        if (itemToGive.Amount == 0) return;
        InteractableName.text = "Give " + itemToGive.Name;
        base.ActivateInteractionUI(collision);
    }
    protected override void DeactivateInteractionUI(Collider2D collision)
    {
        base.DeactivateInteractionUI(collision);
    }
    public void Interact()
    {
        UITrigger.gameObject.SetActive(false);
        InteractionTrigger.gameObject.SetActive(false);
        itemToGive.Amount--;
        IndicatorUI.SetActive(false);

        if (Desintegrate) Desintegrate.PlayFeedbacks();
        if (BlockerCollider) BlockerCollider.enabled = false;
    }

    public void UnInteract()
    {

    }
}
