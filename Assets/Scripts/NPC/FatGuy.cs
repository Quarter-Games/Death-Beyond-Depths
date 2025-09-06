using MoreMountains.Feedbacks;
using System;
using System.Collections;
using UnityEngine;

public class FatGuy : InteractableObject, IInteractable
{
    [SerializeField] InventoryItem itemToGive;
    [SerializeField] MMF_Player Desintegrate;
    [SerializeField] Collider2D BlockerCollider;
    [SerializeField] MMF_Player LoopDialogue;

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
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (itemToGive.Amount == 0) return;
        base.OnTriggerEnter2D(collision);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(PlayLoop());
    }

    private IEnumerator PlayLoop()
    {
        while (true) 
        {
            yield return LoopDialogue.PlayFeedbacksCoroutine(Vector3.one);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
    public bool Interact()
    {
        UITrigger.gameObject.SetActive(false);
        InteractionTrigger.gameObject.SetActive(false);
        itemToGive.Amount--;
        IndicatorUI.SetActive(false);

        if (Desintegrate) StartCoroutine(WaitToDisintegrate());
        if (BlockerCollider) BlockerCollider.enabled = false;
        return true;
    }
    IEnumerator WaitToDisintegrate()
    {
        yield return Desintegrate.PlayFeedbacksCoroutine(Vector3.one);
        gameObject.SetActive(false);
    }
    public void UnInteract()
    {

    }
}
