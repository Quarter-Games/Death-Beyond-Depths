using MoreMountains.Feedbacks;
using System;
using UnityEngine;

public class ClocheInteractable : InteractableObject, IInteractable
{
    [SerializeField] MMF_Player MMF_Player;
    public bool IsInteractable = false;

    public static event Action OnClocheInteracted;

    public void EnableClocheInteractability()
    {
        IsInteractable = true;
    }

    public void DisableClocheInteractability()
    {
        IsInteractable = false;
        IndicatorUI.SetActive(false);
        //??????
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(!IsInteractable) return;
        base.OnTriggerEnter2D(collision);
    }
    
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if(!IsInteractable) return;
        base.OnTriggerExit2D(collision);
    }

    public void Interact()
    {
        if(!IsInteractable) return;
        Debug.Log("Cloche Interacted");
        MMF_Player.PlayFeedbacks();
        IsInteractable = false;
        IndicatorUI.SetActive(false);
        OnClocheInteracted?.Invoke();
    }

    public void UnInteract()
    {
        
    }
}
