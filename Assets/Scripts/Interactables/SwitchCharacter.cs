using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchCharacter : InteratableObject, IInteractable
{
    [SerializeField] GameObject CaptainObject;
    [SerializeField] GameObject GirlObject;

    [ContextMenu("Switch Characters")]
    public void Interact()
    {
        ToggleCharactersActivity();
    }

    public void UnInteract()
    {
        ToggleCharactersActivity();
    }

    private void ToggleCharactersActivity()
    {
        CaptainObject.SetActive(!CaptainObject.activeSelf);
        GirlObject.SetActive(!GirlObject.activeSelf);
    }
}
