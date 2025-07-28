using UnityEngine;

public class EnemySpawner : InteractableObject, IInteractable
{
    [SerializeField] EnemyAI EnemyPrefabToSpawn;
    [SerializeField] Transform SpawnPoint;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {

    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {

    }
    public bool Interact()
    {
        if (EnemyPrefabToSpawn == null || SpawnPoint == null)
        {
            Debug.LogError("EnemyPrefabToSpawn or SpawnPoint is not set.");
            return false;
        }
        Instantiate(EnemyPrefabToSpawn, SpawnPoint.position, SpawnPoint.rotation);
        return true;
    }

    public void UnInteract()
    {
    }
}
