using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchCharacter : InteractableObject, IInteractable
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
        //CaptainObject.SetActive(!CaptainObject.activeSelf);
        //GirlObject.SetActive(!GirlObject.activeSelf);
        if (CaptainObject.activeSelf)
        {
            CaptainObject.SetActive(false);
            CameraController.Instance.Target = GirlObject.transform;
            GirlObject.SetActive(true);
        }
        else
        {
            GirlObject.SetActive(false);
            CameraController.Instance.Target = GirlObject.transform;
            CaptainObject.SetActive(true);
        }
    }
}
