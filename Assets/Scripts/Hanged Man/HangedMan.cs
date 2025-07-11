
using System;
using UnityEngine;

public class HangedMan : Door
{
    [SerializeField] GameObject NoteUI;
    public override void ChangeState(bool isOpen)
    {
        if (isOpen) return;
        base.ChangeState(isOpen);
    }
    public override void Interact()
    {
        base.Interact();
    }
    protected override void LockedFailedToOpen()
    {
        base.LockedFailedToOpen();
        ShowNote();
    }

    private void ShowNote()
    {
        NoteUI.SetActive(true);
    }
}