using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "Scriptable Objects/InventoryItem")]
public class InventoryItem : ScriptableObject
{
    public event System.Action<InventoryItem> AfterAmountChange;
    private static Dictionary<string, int> AmountList = new();
    public string Name;
    [SerializeField] string UNIQUE_ID;
    public string Description;
    [SerializeField] int _startingAmount;
    [SerializeField] int _maxAmount;
    private void OnValidate()
    {
        UNIQUE_ID = UnityEditor.AssetDatabase.GetAssetPath(this);
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
}
