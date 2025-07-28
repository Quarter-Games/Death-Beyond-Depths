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
    public bool Interact()
    {
        if (ObjectToDisappear == null)
        {
            Debug.LogError("ObjectToDisappear is not set.");
            return false;
        }
        ObjectToDisappear.gameObject.SetActive(IsEnabled);
        return true;
    }

    public void UnInteract()
    {

    }
}
