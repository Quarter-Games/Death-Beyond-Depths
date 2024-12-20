using System;
using UnityEngine;

public class InventoryItemObserver : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text _text;
    [SerializeField] UnityEngine.UI.Image _image;
    [SerializeField] protected InventoryItem _item;
    private void OnEnable()
    {
        if (_item != null)
            StartObserve(_item);
    }
    public void StartObserve(InventoryItem item)
    {
        if (_item != null)
            StopObserve();
        if (item == null)
            return;
        _item = item;
        item.AfterAmountChange += OnAmountChange;
        OnAmountChange(item);
    }
    public void StopObserve()
    {
        if (_item == null)
            return;
        _item.AfterAmountChange -= OnAmountChange;
    }
    private void OnDisable()
    {
        StopObserve();
    }
    virtual protected void OnAmountChange(InventoryItem item)
    {
        if (_text != null)
            _text.text = item.Amount.ToString();
        if (_image != null)
            _image.sprite = item.Icon;
    }
}
