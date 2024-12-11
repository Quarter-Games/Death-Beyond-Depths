using UnityEngine;

public abstract class InteratableObject : MonoBehaviour
{
    protected PlayerCharacterController CachedPlayerController;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CachedPlayerController))
        {
            InformManagerOfInteractability();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CachedPlayerController))
        {
            InformManagerOfInteractability();
        }
    }

    public void InformManagerOfInteractability()
    {
        InteractablesManager.Instance.AddInteractableObject(this);
    }
}
