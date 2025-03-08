using UnityEngine;

public class EnemyWaker : InteractableObject, IInteractable
{
    [SerializeField] EnemyAI EnemyToWakeUp;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {

    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {

    }
    public void Interact()
    {
        EnemyToWakeUp.enabled = true;
    }

    public void UnInteract()
    {
    }
}
