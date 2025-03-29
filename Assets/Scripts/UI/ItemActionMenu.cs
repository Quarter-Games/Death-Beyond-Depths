using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemActionMenu : MonoBehaviour
{
    [SerializeField] List<InventoryItemActionButton> Actions;
    public void Init(List<InventoryItemActionData> data,InventoryItem item)
    {
        for (int i = 0; i < Actions.Count; i++)
        {
            if (i < data.Count)
            {
                Actions[i].gameObject.SetActive(true);
                Actions[i].Init(data[i],item);
            }
            else
            {
                Actions[i].gameObject.SetActive(false);
            }
        }
    }
}
