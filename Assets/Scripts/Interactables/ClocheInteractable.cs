using MoreMountains.Feedbacks;
using UnityEngine;

public class ClocheInteractable : InteractableObject, IInteractable
{
    [SerializeField] MMF_Player MMF_Player;
    bool IsInteractable = true;
    
    public void EnableClocheInteractability()
    {
        IsInteractable = true;
    }

    public void DisableClocheInteractability()
    {
        IsInteractable = false;
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
        Debug.Log("Cloche Interacted");
        MMF_Player.PlayFeedbacks();
        IsInteractable = false;
        IndicatorUI.SetActive(false);
    }

    public void UnInteract()
    {
        
    }
}
