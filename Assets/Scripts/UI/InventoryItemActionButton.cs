using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemActionButton : MonoBehaviour
{

    [SerializeField] Button button;
    [SerializeField] TMPro.TMP_Text text;
    [SerializeField] Image image;
    InventoryItemActionData _data;
    InventoryItem InventoryItem;

    private void OnEnable()
    {
        _data = null;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }
    public void Init(InventoryItemActionData data, InventoryItem item)
    {
        InventoryItem = item;
        _data = data;
        text.text = data.ActionName;
        image.sprite = data.Icon;
        button.interactable = _data.IsExecutable(InventoryItem);
    }
    public void OnClick()
    {
        _data.Execute(InventoryItem);
        InventoryManager.Instance.CloseActionMenu();
    }
}
