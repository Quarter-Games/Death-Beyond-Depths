using UnityEngine;

public class ObjectDissapearer : InteractableObject, IInteractable
{
    [SerializeField] bool IsEnabled;
    [SerializeField] GameObject ObjectToDisappear;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {

    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {

    }
    public void Interact()
    {
        if (ObjectToDisappear == null)
        {
            Debug.LogError("ObjectToDisappear is not set.");
            return;
        }
        ObjectToDisappear.gameObject.SetActive(IsEnabled);
    }

    public void UnInteract()
    {

    }
}
