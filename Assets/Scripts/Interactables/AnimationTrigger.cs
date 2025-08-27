
using UnityEngine;

internal class AnimationTrigger : InteractableObject, IInteractable
{
    [SerializeField] private Animator animator;
    [SerializeField] private string triggerName;
    public bool Interact()
    {
        animator.SetTrigger(triggerName);
        return true;
    }
    public void UnInteract()
    {
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }
}
