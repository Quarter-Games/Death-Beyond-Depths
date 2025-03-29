using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "Scriptable Objects/InventoryItem")]
public class InventoryItem : ScriptableObject, IDiscardable
{
    public event System.Action<InventoryItem> AfterAmountChange;
    private static Dictionary<string, int> AmountList = new();
    public string Name;
    [SerializeField] string UNIQUE_ID;
    [Multiline]public string Description;
    [SerializeField] int _startingAmount;
    [SerializeField] int _maxAmount;
    public List<InventoryItemActionData> Actions;
    private void OnValidate()
    {
#if UNITY_EDITOR
        UNIQUE_ID = UnityEditor.AssetDatabase.GetAssetPath(this);
#endif
    }


    public int Amount
    {
        get
        {
            if (AmountList.ContainsKey(UNIQUE_ID))
            {
                return AmountList[UNIQUE_ID];
            }
            else
            {
                AmountList.Add(UNIQUE_ID, _startingAmount);
                return _startingAmount;
            }

        }
        set
        {
            var _amount = value;
            _amount = Mathf.Clamp(_amount, 0, _maxAmount);
            AmountList[UNIQUE_ID] = _amount;
            AfterAmountChange?.Invoke(this);
        }
    }
    public Sprite Icon;
    public void Discard()
    {
        var player = InventoryManager.Instance.Player;
        var pickUp = Instantiate(InventoryManager.Instance.PickUpPrefab, player.transform.position, Quaternion.identity);
        pickUp.Init(this, Amount);
        Amount = 0;
    }
    virtual public bool IsDiscardable()
    {
        return Amount > 0;
    }
}
public interface IDiscardable
{
    void Discard();
    bool IsDiscardable();
}
public interface IUsable
{
    void Use();
    bool IsUsable();
}
public interface IEquipable
{
    void Equip();
    bool IsEquipable();
}