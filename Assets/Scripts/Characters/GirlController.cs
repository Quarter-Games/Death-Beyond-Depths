using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GirlController : PlayerCharacterController
{
    [SerializeField] GameObject Lantern;
    [SerializeField] bool isLanternOn = false;
    protected override void SpecialMove()
    {
        throw new NotImplementedException();
    }
    public override void LeftMouseClick(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
    public override void RightMouseHold(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase);
        if (context.phase == InputActionPhase.Performed && !isLanternOn)
        {
            isLanternOn = true;
            Lantern.SetActive(true);
            CameraController.Instance.ActivateInvisibilityLayer();
        }
        else if (context.phase == InputActionPhase.Canceled || context.phase == InputActionPhase.Performed)
        {
            isLanternOn = false;
            Lantern.SetActive(false);
            CameraController.Instance.DeactivateInvisibilityLayer();
        }
    }

}
