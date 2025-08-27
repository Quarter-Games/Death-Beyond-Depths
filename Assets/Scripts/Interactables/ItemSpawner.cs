
using UnityEngine;

internal class ItemSpawner : InteractableObject, IInteractable
{
    [SerializeField] PickUp PickUpPrefab;
    [SerializeField] InventoryItem ItemToSpawn;
    [SerializeField] Transform SpawnPoint;
    [SerializeField] int AmountToSpawn = 1;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {

    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {

    }
    public bool Interact()
    {
        if (PickUpPrefab == null || SpawnPoint == null || ItemToSpawn == null)
        {
            Debug.LogError("PickUpPrefab, SpawnPoint or ItemToSpawn is not set.", this);
            return false;
        }
        PickUp newPickUp = Instantiate(PickUpPrefab, SpawnPoint.position, SpawnPoint.rotation);
        newPickUp.Init(ItemToSpawn, AmountToSpawn);
        return true;
    }
    public void UnInteract()
    {
    }
}
