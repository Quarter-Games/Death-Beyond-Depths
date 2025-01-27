using UnityEngine;

public class Door : InteractableObject, IInteractable
{
    [SerializeField] Collider2D DoorCollider;
    [SerializeField] Animator animator;
    [Tooltip("Does it have lock on it")]
    public bool isLocked;
    public bool IsOpen;
    private void Awake()
    {
        if (isLocked) return;
        ChangeState(IsOpen);
    }
    public void ChangeState(bool isOpen)
    {
        if (animator != null) animator.SetTrigger(isOpen?"Open":"Close");
        if (DoorCollider != null) DoorCollider.enabled = !isOpen;
        IsOpen = isOpen;
    }

    public void Interact()
    {
        if (isLocked) return;
        ChangeState(!IsOpen);
    }

    public void UnInteract()
    {
        if (isLocked) return;
        ChangeState(!IsOpen);
    }
}
