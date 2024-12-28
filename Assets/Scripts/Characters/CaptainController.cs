using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CaptainController : PlayerCharacterController
{
    [Header("Shooting")]
    [SerializeField] InputActionReference PointerMovementAction;
    [SerializeField] float ShootCooldown = 2;
    [SerializeField] Projectile ProjectilePrefab;
    [SerializeField] Transform ProjectileSpawnPosition;
    [SerializeField] Vector3 mousePos;
    [SerializeField] InventoryItem ShootingItem;
    protected override void OnEnable()
    {
        base.OnEnable();
        PointerMovementAction.action.performed += context => UpdateMousePosition(context);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        PointerMovementAction.action.performed -= context => UpdateMousePosition(context);
    }

    public override void LeftMouseClick(InputAction.CallbackContext context)
    {
        if (IsRaightClickHold && CanShoot()) Shoot(context);
        else if (!IsRaightClickHold) base.LeftMouseClick(context);
    }
    public void UpdateMousePosition(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (ShootingItem.Amount <= 0) return;
        //TODO: Calculate projectile rotation
        //TODO: Stop Using Camera.main
        var camera = Camera.main;

        var proj = Instantiate(ProjectilePrefab, ProjectileSpawnPosition.position, Quaternion.identity);
        var pos = camera.WorldToScreenPoint(ProjectileSpawnPosition.position);

        proj.Init(((Vector2)(mousePos - pos)).normalized);
        IsRangeAttacking = true;
        StartCoroutine(WaitForCoolDown(ShootCooldown, false));
        ShootingItem.Amount--;
    }

    private bool CanShoot()
    {
        return !IsRangeAttacking && !IsMeleeAttacking;
    }

    public override void RightMouseHold(InputAction.CallbackContext context)
    {
        if (context.performed && !IsRaightClickHold) StartAim();
        else if (context.canceled || context.performed) EndAim();
    }

    public void StartAim()
    {
        IsRaightClickHold = true;
    }
    public void EndAim()
    {
        IsRaightClickHold = false;
    }
    protected override void SpecialMove()
    {
        //throw new System.NotImplementedException();
    }
    private Vector3 ConvertMousePosToWorldCoordinates(Vector3 mousePos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = transform.position.z;
        return worldPos;
    }

    protected override void Flip()
    {
        throw new NotImplementedException();
    }
}
