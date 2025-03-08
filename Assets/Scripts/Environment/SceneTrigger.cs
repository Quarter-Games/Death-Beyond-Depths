using System.Collections.Generic;
using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] List<InteractableObject> ObjectsToActivate;
    [SerializeField] InventoryItem item;
    [SerializeField] int RequiredAmount;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (item != null)
        {
            if (item.Amount < RequiredAmount) return;
        }
        if (collision.TryGetComponent<PlayerCharacterController>(out var player))
        {
            foreach (var obj in ObjectsToActivate)
            {
                if (obj is IInteractable interact)
                {
                    interact.Interact();
                }
            }
            gameObject.SetActive(false);
        }
    }
}
