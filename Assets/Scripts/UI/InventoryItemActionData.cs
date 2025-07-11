using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemActionData", menuName = "Scriptable Objects/InventoryItemActionData")]
public class InventoryItemActionData : ScriptableObject
{
    public Sprite Icon;
    public string ActionName;
    public InventoryItemActionType ActionType;
    public bool IsExecutable(InventoryItem item)
    {
        switch (ActionType)
        {
            case InventoryItemActionType.Use:
                return (item is IUsable use) && use.IsUsable();
            case InventoryItemActionType.Equip:
                return (item is IEquipable equipable) && equipable.IsEquipable();
            case InventoryItemActionType.Discard:
                return (item is IDiscardable discardable) && discardable.IsDiscardable();
            default:
                Debug.Log("Action type not implemented");
                return false;
        }
    }
    public void Execute(InventoryItem item)
    {
        switch (ActionType)
        {
            case InventoryItemActionType.Use:
                Debug.Log("Use");
                (item as IUsable)?.Use();
                break;
            case InventoryItemActionType.Equip:
                Debug.Log("Equip");
                (item as IEquipable)?.Equip();
                break;
            case InventoryItemActionType.Discard:
                Debug.Log("Discard");
                (item as IDiscardable)?.Discard();
                break;
            default:
                Debug.Log("Action type not implemented");
                break;
        }
    }
}
public enum InventoryItemActionType
{
    Use,
    Equip,
    Discard,
    Give
}
