
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class HangedMan : InteractableObject, IInteractable
{
    [SerializeField] GameObject NoteUI;
    [SerializeField] Animator animator;
    [SerializeField] InventoryItem Salt;
    [SerializeField] InventoryItem RewardItem;
    [SerializeField] ClochePuzzleManager puzzleManager;
    [SerializeField] MMF_Player IdleLoop;
    [SerializeField] MMF_Player OnMove;
    [SerializeField] Light[] ExemptLights;
    public bool GotNote;

    public UnityEvent OnSaltGiven;
    public UnityEvent OnPuzzleSolved;
    private List<Light> AllLights = new();

    void Start()
    {
        AllLights.AddRange(FindObjectsByType<Light>(FindObjectsSortMode.None).Where(l => l.gameObject.activeInHierarchy));
    }

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
            base.OnTriggerEnter2D(collision);
            IndicatorUI.SetActive(true);
            IndicatorUI.transform.localScale = Vector3.one * 50;
            return;
        }
        else if (state == HangedManState.AfterNote)
        {
            InteractableName.text = "Talk Again";
        }
        else if (state == HangedManState.BeforeReward)
        {
            InteractableName.text = "Give Salt";
            base.OnTriggerEnter2D(collision);
            IndicatorUI.SetActive(true);
            IndicatorUI.transform.localScale = Vector3.one * 50;
            return;

        }
        else if (state == HangedManState.AfterReward) return;
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        var state = CurrentState;
        IndicatorUI.SetActive(false);

        if (state == HangedManState.BeforeNote)
        {
            InteractableName.text = "Talk";
        }
        else if (state == HangedManState.BeforeReward) InteractableName.text = "Give Salt";
        else if (state == HangedManState.AfterReward) return;
        base.OnTriggerExit2D(collision);
    }
    public bool Interact()
    {
        var state = CurrentState;
        if (state == HangedManState.BeforeNote)
        {
            IndicatorUI.SetActive(false);
            animator.SetTrigger("Give");
            StartCoroutine(PlayMoveSound());
            GotNote = true;
            StartCoroutine(waitToTakeNote());

            return true;

        }
        else if (state == HangedManState.BeforeReward)
        {
            InteractionTrigger.gameObject.SetActive(false);
            Salt.Amount--;
            animator.SetTrigger("Salt");
            StartCoroutine(PlayMoveSound());
            StartCoroutine(waitToGetReward());
            puzzleManager.EnableAllCloches();
            TurnOffAllExceptExempt();
            OnSaltGiven?.Invoke();
            IndicatorUI.SetActive(false);
            return true;
        }
        return false;
    }

    private IEnumerator waitToGetReward()
    {
        yield return new WaitUntil(() => RewardItem.Amount != 0);
        OnPuzzleSolved?.Invoke();
        RestoreLights();
        animator.SetTrigger("UnSalt");
        StartCoroutine(PlayMoveSound());
    }

    IEnumerator PlayMoveSound()
    {
        IdleLoop.StopFeedbacks();
        yield return OnMove.PlayFeedbacksCoroutine(Vector3.zero);
        IdleLoop.PlayFeedbacks();
    }

    IEnumerator waitToTakeNote()
    {
        yield return new WaitForSeconds(2.5f);
        NoteUI.SetActive(true);
        yield return new WaitUntil(() => !NoteUI.activeInHierarchy);
        animator.SetTrigger("Take");
        StartCoroutine(PlayMoveSound());
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

    [ContextMenu("light test")]
    public void TurnOffAllExceptExempt()
    {
        foreach (var light in AllLights)
        {
            if (light && Array.IndexOf(ExemptLights, light) == -1)
            {
                light.enabled = false;
            }
        }
        foreach (var light in ExemptLights)
        {
            if (light)
                light.gameObject.SetActive(true);
        }
    }

    public void RestoreLights()
    {
        foreach (var light in AllLights)
        {
            if (light)
            light.enabled = true;
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