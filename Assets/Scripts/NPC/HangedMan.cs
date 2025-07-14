
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using UnityEngine;

public class HangedMan : InteractableObject, IInteractable
{
    [SerializeField] GameObject NoteUI;
    [SerializeField] Animator animator;
    [SerializeField] InventoryItem Salt;
    [SerializeField] InventoryItem RewardItem;
    [SerializeField] ClochePuzzleManager puzzleManager;
    [SerializeField] MMF_Player OnInitAnimation;
    [SerializeField] MMF_Player IdleLoop;
    [SerializeField] MMF_Player OnMove;
    public bool GotNote;

    protected override void ActivateInteractionUI(Collider2D collision)
    {
        if (CurrentState == HangedManState.AfterReward || CurrentState == HangedManState.AfterNote) return;
        base.ActivateInteractionUI(collision);
    }
    protected override void DeactivateInteractionUI(Collider2D collision)
    {
        base.DeactivateInteractionUI(collision);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        var state = CurrentState;
        if (state == HangedManState.BeforeNote)
        {
            InteractableName.text = "Talk";
        }
        else if (state == HangedManState.AfterNote)
        {
            InteractableName.text = "Talk Again";
        }
        else if (state == HangedManState.BeforeReward) InteractableName.text = "Give Salt";
        else if (state == HangedManState.AfterReward) return;
        base.OnTriggerEnter2D(collision);
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        var state = CurrentState;
        if (state == HangedManState.BeforeNote)
        {
            InteractableName.text = "Talk";
        }
        else if (state == HangedManState.BeforeReward) InteractableName.text = "Give Salt";
        else if (state == HangedManState.AfterReward) return;
        base.OnTriggerExit2D(collision);
    }
    public void Interact()
    {
        var state = CurrentState;
        if (state == HangedManState.BeforeNote)
        {
            IndicatorUI.SetActive(false);
            NoteUI.SetActive(true);
            animator.SetTrigger("Give");
            GotNote = true;
            StartCoroutine(waitToTakeNote());

        }
        else if (state == HangedManState.BeforeReward)
        {
            UITrigger.gameObject.SetActive(false);
            InteractionTrigger.gameObject.SetActive(false);
            Salt.Amount--;
            animator.SetTrigger("Salt");
            StartCoroutine(waitToGetReward());
            puzzleManager.EnableAllCloches();
        }
    }

    private IEnumerator waitToGetReward()
    {
        yield return new WaitUntil(() => RewardItem.Amount != 0);
        animator.SetTrigger("UnSalt");
    }

    IEnumerator waitToTakeNote()
    {
        yield return new WaitUntil(() => !NoteUI.activeInHierarchy);
        animator.SetTrigger("Take");
    }
    public void UnInteract()
    {

    }
    public HangedManState CurrentState
    {
        get
        {
            if (Salt.Amount == 0 && RewardItem.Amount == 0 && !GotNote) return HangedManState.BeforeNote;
            else if (Salt.Amount == 0 && RewardItem.Amount == 0 && GotNote) return HangedManState.AfterNote;
            else if (Salt.Amount > 0 && RewardItem.Amount == 0 && GotNote) return HangedManState.BeforeReward;
            else return HangedManState.AfterReward;
        }
    }
}
public enum HangedManState
{
    BeforeNote,
    AfterNote,
    BeforeReward,
    AfterReward
}