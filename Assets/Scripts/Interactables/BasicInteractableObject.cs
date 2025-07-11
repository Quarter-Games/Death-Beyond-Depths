using MoreMountains.Feedbacks;
using UnityEngine;

public class BasicInteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField]  GameObject IndicatorUI;
    [SerializeField] CustomTrigger UITrigger;
    [SerializeField] CustomTrigger IndicatorTrigger;
    [SerializeField] MMF_Player MMF_Player;
    [SerializeField] private float MinimumUISize = 0.5f;

    private Vector3 OriginalScale;
    protected bool IsWithinPlayerRange;
    private float maxDistance = 5f;
    private CanvasGroup CanvasGroup;
    PlayerCharacterController CachedPlayerController;

    private void OnValidate()
    {
        if (MMF_Player == null)
        {
            MMF_Player = GetComponentInChildren<MMF_Player>();
        }
    }

    void OnEnable()
    {
        if (UITrigger != null)
        {
            UITrigger.EnterTrigger += ActivateInteractionUI;
            UITrigger.ExitTrigger += DeactivateInteractionUI;
        }
    }

    void OnDisable()
    {
        if (UITrigger != null)
        {
            UITrigger.EnterTrigger -= ActivateInteractionUI;
            UITrigger.ExitTrigger -= DeactivateInteractionUI;
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
        }
    }

    public void Interact()
    {
        MMF_Player.PlayFeedbacks();
    }

    public void UnInteract()
    {
        
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
        //if (IndicatorKey != null)
        //{
        //    IndicatorKey.gameObject.SetActive(false);
        //}
    }

    private void Update()
    {
        if (!IsWithinPlayerRange || CachedPlayerController == null || IndicatorUI == null) return;
        if (!IndicatorUI.activeSelf)
        {
            IndicatorUI.SetActive(true);
        }
        float distance = Vector3.Distance(CachedPlayerController.transform.position, transform.position);
        float scaleFactor = Mathf.Clamp01(1 - (distance / maxDistance));
        float newScale = Mathf.Lerp(MinimumUISize, 1, scaleFactor);
        IndicatorUI.transform.localScale = OriginalScale * newScale;
        if (CanvasGroup != null)
        {
            CanvasGroup.alpha = Mathf.Lerp(0.5f, 1, scaleFactor);
        }
    }
}
