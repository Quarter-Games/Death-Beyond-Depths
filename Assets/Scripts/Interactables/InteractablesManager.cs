using System.Collections.Generic;
using UnityEngine;

public class InteractablesManager : MonoBehaviour
{
    public static InteractablesManager Instance { get; private set; }

    private IInteractable CurrentInteractableObject {
        get
        {
            return Interactables[0];
        }
    }
    private List<IInteractable> Interactables;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(this);
        Interactables = new();
    }

    public void AddInteractableObject(IInteractable interactable)
    {
        if (interactable == null)
            return;
        if(Interactables.Contains(interactable))
        {
            Interactables.Remove(interactable);
            return;
        }
        Interactables.Add(interactable);
    }

    public void Interact()
    {
        if(Interactables.Count != 0)
            CurrentInteractableObject.Interact();
    }
    public bool IsInteractingWithClimable()
    {
        if (Interactables.Count == 0 || CurrentInteractableObject is not ClimablePoint)
            return false;
        return true;
    }
}
