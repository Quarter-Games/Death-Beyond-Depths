using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; } = null;

    [SerializeField] private Vector3 SpawnPoint;
    private List<InventoryItem> ItemsToSpawnWith = new List<InventoryItem>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetCheckpoint(Transform transformPoint, List<InventoryItem> itemsToSpawnWith)
    {
        SpawnPoint = transformPoint.position;
        ItemsToSpawnWith = itemsToSpawnWith;
    }

    public void SpawnPlayerAtCheckpoint(PlayerCharacterController player)
    {
        Debug.Log("Spawning player at checkpoint");
        if (player && SpawnPoint != Vector3.zero)
        {
            player.transform.position = SpawnPoint;
        }
        else return;
        foreach (var item in ItemsToSpawnWith)
        {
            item.Amount = 1;
            InventoryManager.Instance._items.Add(item);
        }
    }
}
