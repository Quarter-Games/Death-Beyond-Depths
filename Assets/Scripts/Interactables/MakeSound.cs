
using UnityEngine;
using UnityEngine.Audio;

internal class MakeSound : InteractableObject, IInteractable
{
    [SerializeField] AudioResource SFX;
    [SerializeField] Transform SpawnPoint;
    public void Interact()
    {

        AudioManager.Instance.PlaySoundEffect(SFX, SpawnPoint);
    }

    public void UnInteract()
    {
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }
}