using UnityEngine;

public class PickUp : InteractableObject, IInteractable
{
    [SerializeField] InventoryItem item;
    [SerializeField] int AmountToPickUp = 1;
    [SerializeField] SpriteRenderer spriteRenderer;
    private void OnEnable()
    {
        if (item == null)
        {
            Debug.LogWarning("Item is not assigned to the PickUp script", this);
            return;
        }
        spriteRenderer.sprite = item.Icon;
    }
    public void Interact()
    {
        item.Amount += AmountToPickUp;
        Destroy(gameObject);
    }

    public void UnInteract()
    {

    }
}
