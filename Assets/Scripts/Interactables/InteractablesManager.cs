using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class InteractablesManager : MonoBehaviour
{
    public static InteractablesManager Instance { get; private set; }
    [SerializeField] AudioResource OnInteraction;

    private IInteractable CurrentInteractableObject
    {
        get
        {
            return Interactables[0];
        }
    }
    private List<IInteractable> Interactables;

    private void Awake()
    {
        if (Instance)
        {
            Instance.Interactables.Clear();
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
        if (Interactables.Contains(interactable))
        {
            return;
        }
        Interactables.Add(interactable);
    }
    public void RemoveInteractableObject(IInteractable interactable)
    {
        if (interactable == null)
            return;
        if (Interactables.Contains(interactable))
        {
            Interactables.Remove(interactable);
        }
    }
    public bool Interact()
    {
        if (Interactables.Count != 0)
        {
            // Decided to play player interaction SFX from here, cause it is the only place when we can sure, that he is interacting indeed
            AudioManager.Instance.PlaySoundEffect(OnInteraction, (CurrentInteractableObject as MonoBehaviour).transform);
            return CurrentInteractableObject.Interact();


        }
        return false;
    }
    public bool IsInteractingWithClimable()
    {
        if (Interactables.Count == 0 || CurrentInteractableObject is not ClimablePoint)
            return false;
        return true;
    }
    public bool IsCurrentInteractableObject(IInteractable interactable)
    {
        if (Interactables.Count != 0)
            return CurrentInteractableObject == interactable;
        return false;
    }
}
