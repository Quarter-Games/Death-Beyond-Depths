using UnityEngine;

public class SwitchCharacter : InteratableObject, IInteractable
{
    [SerializeField] GameObject CaptainObject;
    [SerializeField] GameObject GirlObject;

    public void Interact()
    {
        CaptainObject.SetActive(!CaptainObject.activeSelf);
        GirlObject.SetActive(!GirlObject.activeSelf);
    }

    public void UnInteract()
    {
        CaptainObject.SetActive(!CaptainObject.activeSelf);
        GirlObject.SetActive(!GirlObject.activeSelf);
    }
}
