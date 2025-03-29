using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class InteractableObject : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject IndicatorUI;
    [SerializeField] private float MinimumUISize = 0.5f;
    [SerializeField] TextMeshProUGUI IndicatorKey;
    [SerializeField] CustomTrigger UITrigger;
    [SerializeField] CustomTrigger InteractionTrigger;
    public TMP_Text InteractableName;

    protected PlayerCharacterController CachedPlayerController;
    private Vector3 OriginalScale;
    protected bool IsWithinPlayerRange;
    private float maxDistance = 5f;
    private CanvasGroup CanvasGroup;

    protected virtual void OnEnable()
    {
        if (UITrigger != null)
        {
            UITrigger.EnterTrigger += ActivateInteractionUI;
            UITrigger.ExitTrigger += DeactivateInteractionUI;
        }
        if (InteractionTrigger != null)
        {
            InteractionTrigger.EnterTrigger += QueueForInteractability;
            InteractionTrigger.ExitTrigger += QueueForInteractability;
        }
    }

    protected virtual void OnDisable()
    {
        if (UITrigger != null)
        {
            UITrigger.EnterTrigger -= ActivateInteractionUI;
            UITrigger.ExitTrigger -= DeactivateInteractionUI;
        }
        if (InteractionTrigger != null)
        {
            InteractionTrigger.EnterTrigger -= QueueForInteractability;
            InteractionTrigger.ExitTrigger -= QueueForInteractability;
        }
    }

    private void Start()
    {
        if (IndicatorUI != null)
        {
            OriginalScale = IndicatorUI.transform.localScale;
            if (IndicatorUI.transform.parent.TryGetComponent(out CanvasGroup))
            {
                CanvasGroup.alpha = 0;
            }
            IndicatorUI.SetActive(false);
            IndicatorKey.gameObject.SetActive(false);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out CachedPlayerController))
        {
            return;
        }
        InformManagerOfInteractability();
    }

    private void ActivateInteractionUI(Collider2D collision)
    {
        if (!collision.TryGetComponent(out CachedPlayerController))
        {
            return;
        }
        if (IndicatorUI != null)
        {
            IndicatorUI.SetActive(true);
            IsWithinPlayerRange = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out CachedPlayerController))
        {
            return;
        }
        InformManagerOfInteractability();
    }

    private void DeactivateInteractionUI(Collider2D collision)
    {
        if (!collision.TryGetComponent(out CachedPlayerController))
        {
            return;
        }
        if (IndicatorUI != null)
        {
            IndicatorUI.SetActive(false);
            IsWithinPlayerRange = false;
        }
        if (IndicatorKey != null)
        {
            IndicatorKey.gameObject.SetActive(false);
        }
    }

    private void QueueForInteractability(Collider2D collision)
    {
        if (!collision.TryGetComponent(out CachedPlayerController))
        {
            return;
        }
        if (IndicatorKey != null && InteractablesManager.Instance.IsCurrentInteractableObject(this as IInteractable))
        {
            if ((this is Door door) && door.CantBeUnlocked)
            {
                IndicatorKey.gameObject.SetActive(false);
                return;
            }
            IndicatorKey.gameObject.SetActive(!IndicatorKey.gameObject.activeSelf);
        }
    }

    public void InformManagerOfInteractability()
    {
        InteractablesManager.Instance.AddInteractableObject(this as IInteractable);
    }

    private void Update()
    {
        if (!IsWithinPlayerRange || CachedPlayerController == null || IndicatorUI == null) return;
        float distance = Vector3.Distance(CachedPlayerController.transform.position, transform.position);
        float scaleFactor = Mathf.Clamp01(1 - (distance / maxDistance));
        float newScale = Mathf.Lerp(MinimumUISize, 1, scaleFactor);
        IndicatorUI.transform.localScale = OriginalScale * newScale;
        if (CanvasGroup != null)
        {
            CanvasGroup.alpha = Mathf.Lerp(0, 1, scaleFactor);
        }
    }
}
