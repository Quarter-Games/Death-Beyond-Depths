using System;
using UnityEngine;

public class CaptainDialogDoorOpener : MonoBehaviour
{
    [SerializeField] Door door;
    private void OnEnable()
    {
        DialogueController.OnDialogueEnd += UnlockDoor;
    }

    private void UnlockDoor()
    {
        door.Unlock(true);
        door.Interact();
    }

    private void OnDisable()
    {
        DialogueController.OnDialogueEnd -= UnlockDoor;
    }
}
