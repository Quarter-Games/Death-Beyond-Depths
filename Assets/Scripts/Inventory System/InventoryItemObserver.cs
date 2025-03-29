using System;
using System.Collections;
using UnityEngine;

public class InventoryItemObserver : MonoBehaviour
{
    public static event Action<InventoryItemObserver> OnPointerClickEvent;
    [SerializeField] GameObject Glow;
    [SerializeField] TMPro.TMP_Text _text;
    [SerializeField] UnityEngine.UI.Image _image;
    public InventoryItem _item;
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
        {
            OnAmountChange(null);
            return;
        }
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
        CloseGlow();
    }
    public void CloseGlow()
    {
        Glow.SetActive(false);
    }
    virtual protected void OnAmountChange(InventoryItem item)
    {
        CloseGlow();
        if (item == null || item.Amount == 0)
        {
            _image.gameObject.SetActive(false);
            _text.text = "";
        }
        else if (item.Amount == 1)
        {
            _image.gameObject.SetActive(true);
            _image.sprite = item.Icon;
            _text.text = "";
        }
        else
        {
            _image.gameObject.SetActive(true);
            _text.text = item.Amount.ToString();
            _image.sprite = item.Icon;
        }
    }
    public void OnPointerClick()
    {
        if (_item == null)
            return;
        if (_item.Amount == 0)
            return;
        Glow.SetActive(true);
        OnPointerClickEvent?.Invoke(this);
    }
}
