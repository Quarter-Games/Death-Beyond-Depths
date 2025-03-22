using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] GameObject IndicatorUI;
    protected PlayerCharacterController CachedPlayerController;

    private void Start()
    {
        if (IndicatorUI != null)
        {
            IndicatorUI.SetActive(false);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out CachedPlayerController))
        {
            return;
        }
        //TODO - Interact indicator
        if (IndicatorUI != null)
        {
            IndicatorUI.SetActive(true);
        }
        InformManagerOfInteractability();
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out CachedPlayerController))
        {
            return;
        }
        if (IndicatorUI != null)
        {
            IndicatorUI.SetActive(false);
        }
        InformManagerOfInteractability();
    }

    public void InformManagerOfInteractability()
    {
        InteractablesManager.Instance.AddInteractableObject(this as IInteractable);
    }
}
