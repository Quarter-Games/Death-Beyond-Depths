using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] List<InventoryItem> _items = new List<InventoryItem>();
    [SerializeField] List<InventoryItemObserver> Observers = new();
    private void OnEnable()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            Observers[i].StartObserve(_items[i]);
        }
    }
    private void OnDisable()
    {
        foreach (var observer in Observers)
        {
            observer.StopObserve();
        }
    }
}
