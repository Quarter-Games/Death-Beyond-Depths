using UnityEngine;

public class HealItem : InventoryItem, IUsable
{
    public bool IsUsable()
    {
        return true;
    }

    public void Use()
    {
        InventoryManager.Instance.Player.Heal(1);
        Amount = 0; 
        InventoryManager.Instance.CloseInventory(default);
    }
}
