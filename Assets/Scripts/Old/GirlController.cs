
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GirlController : PlayerCharacterController
{
    [SerializeField] GameObject Lantern;
    protected override void SpecialMove()
    {
        throw new NotImplementedException();
    }
    public override void LeftMouseClick(InputAction.CallbackContext context)
    {
        base.LeftMouseClick(context);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        Lantern.SetActive(false);
    }
    public override void RightMouseHold(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase);
        if (context.phase == InputActionPhase.Performed && !IsRaightClickHold)
        {
            IsRaightClickHold = true;
            Lantern.SetActive(true);
            CameraController.Instance.ActivateInvisibilityLayer();
        }
        else if (context.phase == InputActionPhase.Canceled || context.phase == InputActionPhase.Performed)
        {
            IsRaightClickHold = false;
            Lantern.SetActive(false);
            CameraController.Instance.DeactivateInvisibilityLayer();
        }
    }

    protected override void Flip()
    {
        throw new NotImplementedException();
    }
}