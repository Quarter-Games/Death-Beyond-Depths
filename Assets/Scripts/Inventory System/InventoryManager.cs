using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public List<InventoryItem> _items = new List<InventoryItem>();
    public List<InventoryItemObserver> Observers = new();
    [SerializeField] InputActionReference _openInventory;
    [SerializeField] InputActionReference _closeInventory;
    [SerializeField] GameObject InventoryScreen;
    private InventoryItemObserver _selectedItem;
    [SerializeField] ItemActionMenu ActionsScreen;
    public PlayerCharacterController Player;
    public PickUp PickUpPrefab;
    [SerializeField] GameObject descriptionParent;
    [SerializeField] TMPro.TMP_Text descriptionText;
    [SerializeField] TMPro.TMP_Text itemNameText;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Player = FindObjectsByType<PlayerCharacterController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)[0];
    }
    private void OnEnable()
    {
        foreach (var item in _items)
        {
            if (item == null) continue;
            item.Reset();
        }
        _openInventory.action.performed += OpenInventory;
        _closeInventory.action.performed += CloseInventory;
        _openInventory.action.Enable();
        _closeInventory.action.Enable();
        InventoryItemObserver.OnPointerClickEvent += OnItemClick;
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i] == null) continue;
            _items[i].AfterAmountChange += ItemChange;
        }
        StartObserving();
    }

    private void OnItemClick(InventoryItemObserver observer)
    {
        if (_selectedItem == observer) return;
        if (_selectedItem != null)
        {
            _selectedItem.CloseGlow();
        }
        _selectedItem = observer;
        Debug.Log($"Item Clicked <color=green>{observer.name}</color>");
        SetUpActions();
    }

    public void SetUpActions()
    {
        ActionsScreen.gameObject.SetActive(true);
        (ActionsScreen.transform as RectTransform).position = _selectedItem.transform.position;
        ActionsScreen.Init(_selectedItem._item.Actions, _selectedItem._item);
        descriptionParent.gameObject.SetActive(true);
        descriptionText.text = _selectedItem._item.Description;
        itemNameText.text = _selectedItem._item.Name;
    }
    public void CloseActionMenu()
    {
        ActionsScreen.gameObject.SetActive(false);
        descriptionParent.gameObject.SetActive(false);
    }
    public void CloseInventory(InputAction.CallbackContext context)
    {
        InventoryScreen.SetActive(false);
        Time.timeScale = 1;
    }
    public void OpenInventory(InputAction.CallbackContext context)
    {
        _selectedItem = null;
        InventoryScreen.SetActive(!InventoryScreen.activeSelf);
        Time.timeScale = InventoryScreen.activeSelf ? 0 : 1;
        CloseActionMenu();
    }

    private void ItemChange(InventoryItem item)
    {
        StartObserving();
    }

    public void StartObserving()
    {
        var items = _items.Where(x => x.Amount > 0).ToList();
        for (int i = 0; i < Observers.Count; i++)
        {
            if (i >= items.Count)
            {
                Observers[i].StartObserve(null);
                continue;
            }
            Observers[i].StartObserve(items[i]);
        }
    }
    private void OnDisable()
    {
        _openInventory.action.performed -= OpenInventory;
        _closeInventory.action.performed -= CloseInventory;
        _openInventory.action.Disable();
        _closeInventory.action.Disable();
        InventoryItemObserver.OnPointerClickEvent -= OnItemClick;
        for (int i = 0; i < _items.Count; i++)
        {
            _items[i].AfterAmountChange -= ItemChange;
        }
        foreach (var observer in Observers)
        {
            observer.StopObserve();
        }
    }
    public void UseItem()
    {
        Debug.Log($"Use Item <color=cyan>{_selectedItem.name}</color>");
    }
    public void DiscardItem()
    {
        Debug.Log($"Discard Item <color=red>{_selectedItem.name}</color>");
    }
}
