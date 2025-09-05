using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] Transform SpawnPoint;
    [SerializeField] List<InventoryItem> ItemsToSpawnWith;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerCharacterController>() != null)
        {
            CheckpointManager.Instance.SetCheckpoint(SpawnPoint, ItemsToSpawnWith);
            Destroy(gameObject);
        }
    }
}
