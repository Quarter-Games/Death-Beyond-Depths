using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : InteractableObject, IInteractable
{
    public static List<Door> AllDoors = new List<Door>();
    [Space(15f)]
    [SerializeField] Collider2D DoorCollider;
    [SerializeField] protected Animator animator;
    [SerializeField] GameObject LockedUI;
    [Tooltip("Does it have lock on it")]
    public InventoryItem KeyToOpen;
    public bool isLocked;
    public bool IsOpen;
    [SerializeField, Min(1)] int HP = 10;
    [SerializeField] Collider2D CameraBoundary;
    GameObject UnlockedUI;
    public UnityEvent OnUnlock;

    public bool IsBroken => HP < 0 || IsOpen;
    public bool CantBeUnlocked => (isLocked && KeyToOpen?.Amount == 0);
    protected override void OnEnable()
    {
        base.OnEnable();
        AllDoors.Add(this);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        AllDoors.Remove(this);
    }
    private void Awake()
    {
        UnlockedUI = IndicatorUI;
        if (isLocked)
        {
            IndicatorUI = LockedUI;
            return;
        }
        IndicatorUI = UnlockedUI;
        ChangeState(IsOpen);
    }
    virtual public void ChangeState(bool isOpen)
    {
        if (animator != null) animator.SetTrigger(isOpen ? "Open" : "Close");
        if (DoorCollider != null)
        {
            DoorCollider.enabled = !isOpen;
            if (CameraBoundary != null && !isLocked) CameraBoundary.enabled = !isOpen;
        }
        IsOpen = isOpen;
    }
    public void Unlock()
    {
        if (KeyToOpen?.Amount > 0)
        {
            IndicatorUI = UnlockedUI;
            LockedUI.SetActive(false);
            isLocked = false;
            OnUnlock?.Invoke();
        }
    }
    public bool IsInPlayerRange()
    {
        return IsWithinPlayerRange;
    }
    virtual public void Interact()
    {
        if (isLocked)
        {
            if (KeyToOpen.Amount > 0)
            {
                InventoryManager.Instance.OpenInventory(default);
            }
            else
            {
                LockedFailedToOpen();
            }
            return;
        }
        AudioManager.Instance.PlaySoundEffect(OnSuccesfullInteraction, transform);
        ChangeState(!IsOpen);
    }

    protected virtual void LockedFailedToOpen()
    {
        AudioManager.Instance.PlaySoundEffect(OnFailedInteraction, transform);
    }

    virtual public void UnInteract()
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
