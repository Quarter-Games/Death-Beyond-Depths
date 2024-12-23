using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "Scriptable Objects/InventoryItem")]
public class InventoryItem : ScriptableObject
{
    public event System.Action<InventoryItem> AfterAmountChange;
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
        get => PlayerPrefs.GetInt(UNIQUE_ID, _startingAmount);
        set
        {
            var _amount = value;
            _amount = Mathf.Clamp(_amount, 0, _maxAmount);
            PlayerPrefs.SetInt(UNIQUE_ID, _amount);
            AfterAmountChange?.Invoke(this);
        }
    }
    public Sprite Icon;
}
