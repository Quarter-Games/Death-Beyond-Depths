
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
    protected override void OnEnable()
    {
        animator.SetTrigger("Drop");
        base.OnEnable();
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        var state = CurrentState;
        if (state == HangedManState.BeforeNote) InteractableName.text = "Talk";
        else if (state == HangedManState.BeforeReward) InteractableName.text = "Give Salt";
        else if (state == HangedManState.AfterReward) return;
        base.OnTriggerEnter2D(collision);
    }
    public void Interact()
    {
        var state = CurrentState;
        if (state == HangedManState.BeforeNote)
        {
            NoteUI.SetActive(true);
            animator.SetTrigger("Give");
            StartCoroutine(waitToTakeNote());
        }
        else if (state == HangedManState.BeforeReward)
        {
            Salt.Amount--;
            puzzleManager.EnableAllCloches();
        }
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
            if (Salt.Amount == 0 && RewardItem.Amount == 0)
                return HangedManState.BeforeNote;
            else if (Salt.Amount > 0 && RewardItem.Amount == 0)
                return HangedManState.BeforeReward;
            else
                return HangedManState.AfterReward;
        }
    }
}
public enum HangedManState
{
    BeforeNote,
    BeforeReward,
    AfterReward
}