using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CaptainController : PlayerCharacterController
{
    [Header("Shooting")]
    [SerializeField] float ShootCooldown = 2;
    [SerializeField] private bool isAiming = false;
    [SerializeField] Projectile ProjectilePrefab;
    [SerializeField] Transform ProjectileSpawnPosition;
    [SerializeField] Transform AimPosition;
    [SerializeField] Vector3 mousePos;
    [SerializeField] float LastShootTime = -2;
    public override void LeftMouseClick(InputAction.CallbackContext context)
    {
        if (isAiming && CanShoot()) Shoot(context);
    }
    public void UpdateMousePosition(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }

    private void Shoot(InputAction.CallbackContext context)
    {



        //TODO: Calculate projectile rotation
        //TODO: Stop Using Camera.main
        var camera = Camera.main;

        var proj = Instantiate(ProjectilePrefab, ProjectileSpawnPosition.position, Quaternion.identity);
        var pos = camera.WorldToScreenPoint(ProjectileSpawnPosition.position);

        AimPosition.position = new Vector3(pos.x, pos.y, transform.position.z);
        proj.Init(((Vector2)(mousePos - pos)).normalized);
        LastShootTime = Time.time;
    }

    private bool CanShoot()
    {
        if (Time.time - LastShootTime > ShootCooldown) return true;
        return false;
    }

    public override void RightMouseHold(InputAction.CallbackContext context)
    {
        if (context.performed && !isAiming) StartAim();
        else if (context.canceled || context.performed) EndAim();
    }

    public void StartAim()
    {
        isAiming = true;
    }
    public void EndAim()
    {
        isAiming = false;
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
}
