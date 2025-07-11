using UnityEngine;

public class ClocheInteractable : InteractableObject
{
    bool IsInteractable = false;
    
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
}
