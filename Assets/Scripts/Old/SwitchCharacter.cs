using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchCharacter : InteractableObject, IInteractable
{
    [SerializeField] GameObject CaptainObject;
    [SerializeField] GameObject GirlObject;

    [ContextMenu("Switch Characters")]
    public bool Interact()
    {
        ToggleCharactersActivity();
        return true;
    }

    public void UnInteract()
    {
        ToggleCharactersActivity();
    }

    private void ToggleCharactersActivity()
    {
        //CaptainObject.SetActive(!CaptainObject.activeSelf);
        //GirlObject.SetActive(!GirlObject.activeSelf);
        if (CaptainObject.activeSelf)
        {
            CaptainObject.SetActive(false);
            GirlObject.SetActive(true);
        }
        else
        {
            CaptainObject.SetActive(true);
            GirlObject.SetActive(false);
        }
    }
}
