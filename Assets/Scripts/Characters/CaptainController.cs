using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.IK;

public class CaptainController : PlayerCharacterController
{
    [Header("Shooting")]
    [SerializeField] InputActionReference PointerMovementAction;
    [SerializeField] Transform GunIKTarget;
    [SerializeField] LimbSolver2D GunArmSolver;
    [SerializeField] float ShootCooldown = 2;
    [SerializeField] Projectile ProjectilePrefab;
    [SerializeField] Transform ProjectileSpawnPosition;
    [SerializeField] Vector3 mousePos;
    [SerializeField] ParticleSystem OnShootParticles;
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
        if (IsRaightClickHold && CanShoot()) ShootAnimation();
        else if (!IsRaightClickHold) base.LeftMouseClick(context);
    }

    private void ShootAnimation()
    {
        animator.SetTrigger("Shoot");
    }

    public void UpdateMousePosition(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }

    public void Shoot()
    {
        if (ShootingItem.Amount <= 0) return;
        //TODO: Calculate projectile rotation
        //TODO: Stop Using Camera.main
        var camera = Camera.main;

        var proj = Instantiate(ProjectilePrefab, ProjectileSpawnPosition.position, Quaternion.identity);
        var pos = camera.WorldToScreenPoint(ProjectileSpawnPosition.position);

        var direction = ((Vector2)(mousePos - pos)).normalized;
        proj.Init(direction);
        IsRangeAttacking = true;
        StartCoroutine(WaitForCoolDown(ShootCooldown, false));
        ShootingItem.Amount--;
        var trans = Instantiate(OnShootParticles, ProjectileSpawnPosition.position, Quaternion.Euler(direction));
        trans.Play();
    }


    private bool CanShoot()
    {
        return !IsRangeAttacking && !IsMeleeAttacking && ShootingItem.Amount > 0;
    }

    public override void RightMouseHold(InputAction.CallbackContext context)
    {
        if (context.performed && !IsRaightClickHold) StartAim();
        else if (context.canceled || context.performed) EndAim();
    }
    public void EquipGun()
    {
        IsRaightClickHold = true;
    }
    public void StartAim()
    {
        animator.SetTrigger("Enable Gun");
    }
    public void EndAim()
    {
        IsRaightClickHold = false;
        animator.SetTrigger("Remove Gun");
    }
    public void LateUpdate()
    {
        animator.SetBool("IsGunEquiped", IsRaightClickHold);
        if (IsRaightClickHold)
        {

            var worldPos = ConvertMousePosToWorldCoordinates(Input.mousePosition);

            GunIKTarget.position = worldPos;
            GunArmSolver.UpdateIK(1);
            SetRotationTo(worldPos - transform.position);
        }
    }
    protected override void SpecialMove()
    {
        //throw new System.NotImplementedException();
    }
    private Vector3 ConvertMousePosToWorldCoordinates(Vector3 mousePos)
    {
        var camera = Camera.main;
        if (camera == null) return Vector3.zero;

        Ray ray = camera.ScreenPointToRay(mousePos);
        Plane zPlane = new Plane(Vector3.forward, new Vector3(0, 0, transform.position.z));
        if (zPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }
}
