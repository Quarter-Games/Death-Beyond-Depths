using UnityEngine;

[CreateAssetMenu(fileName = "HealingItem", menuName = "Scriptable Objects/HealingItem")]
public class HealItem : InventoryItem, IUsable
{
    public bool IsUsable()
    {
        return InventoryManager.Instance.Player.CanHeal();
    }

    public void Use()
    {
        InventoryManager.Instance.Player.Heal(1);
        Amount = 0; 
        InventoryManager.Instance.CloseInventory(default);
    }
}
