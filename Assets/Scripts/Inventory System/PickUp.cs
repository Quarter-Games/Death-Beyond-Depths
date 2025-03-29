using UnityEngine;

public class PickUp : InteractableObject, IInteractable
{
    [SerializeField] InventoryItem item;
    [SerializeField] int AmountToPickUp = 1;
    [SerializeField] SpriteRenderer spriteRenderer;
    public void Init(InventoryItem item, int amount)
    {
        this.item = item;
        spriteRenderer.sprite = item.Icon;
        AmountToPickUp = amount;
        base.InteractableName.text = item.Name;
    }
    override protected void OnEnable()
    {
        base.OnEnable();
        if (item == null)
        {
            Debug.LogWarning("Item is not assigned to the PickUp script", this);
            return;
        }
        spriteRenderer.sprite = item.Icon;
        base.InteractableName.text = item.Name;
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
