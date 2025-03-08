using System;
using UnityEngine;

public class Door : InteractableObject, IInteractable
{
    [SerializeField] Collider2D DoorCollider;
    [SerializeField] Animator animator;
    [Tooltip("Does it have lock on it")]
    [SerializeField] InventoryItem KeyToOpen;
    public bool isLocked;
    public bool IsOpen;
    [SerializeField, Min(1)] int HP = 10;

    public bool IsBroken => HP < 0 || IsOpen;

    private void Awake()
    {
        if (isLocked) return;
        ChangeState(IsOpen);
    }
    public void ChangeState(bool isOpen)
    {
        if (animator != null) animator.SetTrigger(isOpen ? "Open" : "Close");
        if (DoorCollider != null) DoorCollider.enabled = !isOpen;
        IsOpen = isOpen;
    }

    public void Interact()
    {
        if (isLocked)
        {
            if (KeyToOpen.Amount > 0)
            {
                isLocked = false;
            }
            else return;
        }
        ChangeState(!IsOpen);
    }

    public void UnInteract()
    {
        if (isLocked) return;
        ChangeState(!IsOpen);
    }

    internal void TakeDamage(int v)
    {
        if (IsOpen) return;
        HP -= v;
        if (HP <= 0)
        {
            ChangeState(true);
            HP = 10;
        }
    }
}
