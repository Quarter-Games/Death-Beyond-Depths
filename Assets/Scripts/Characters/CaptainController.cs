using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.Animation;
using UnityEngine.U2D.IK;

public class CaptainController : PlayerCharacterController
{
    [Header("Shooting")]
    public RangeAttack ShootingStats;
    [SerializeField] InputActionReference PointerMovementAction;
    [SerializeField] Transform GunIKTarget;
    [SerializeField] LimbSolver2D GunArmSolver;
    [SerializeField] float ShootCooldown = 2;
    [SerializeField] Projectile ProjectilePrefab;
    [SerializeField] Transform ProjectileSpawnPosition;
    [SerializeField] Vector3 mousePos;
    [SerializeField] ParticleSystem OnShootParticles;
    [SerializeField] InventoryItem ShootingItem;
    [SerializeField] SpriteResolver LeftPalm;
    [SerializeField] Transform ElbowPosition;
    bool isrightClickPerformed;
    Vector3 LastIKPosition;

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
        //TODO: Stop Using Camera.main
        var camera = Camera.main;

        //var proj = Instantiate(ProjectilePrefab, ProjectileSpawnPosition.position, Quaternion.identity);
        var pos = ProjectileSpawnPosition.position;

        var direction = ((Vector2)(pos - ElbowPosition.position)).normalized;
        //proj.Init(direction);
        var rotation = Quaternion.LookRotation(Vector3.forward, direction);
        rotation *= Quaternion.Euler(0, 0, -90); // Rotate 90 degrees to the right

        IsRangeAttacking = true;
        StartCoroutine(WaitForCoolDown(ShootCooldown, false));
        ShootingItem.Amount--;
        var trans = Instantiate(OnShootParticles, ProjectileSpawnPosition.position, rotation);
        trans.Play();
        ShootingRaycast(pos, direction);

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(ProjectileSpawnPosition.position, ProjectileSpawnPosition.position + ((Vector3)(ProjectileSpawnPosition.position - ElbowPosition.position)).normalized);
    }
    public void ShootingRaycast(Vector3 pos, Vector3 direction)
    {
        List<RaycastHit2D> hits = new();
        Physics2D.Raycast(pos, direction, new ContactFilter2D().NoFilter(), hits);
        Debug.Log(hits.Count);
        foreach (var h in hits)
        {
            if (h == default) break;
            if (h.collider != null)
            {
                if (h.collider.TryGetComponent(out EnemyAI damageable))
                {
                    damageable.TakeDamage(ShootingStats.Damage);
                    damageable.Stagger();
                    Debug.LogWarning("Hit Enemy");
                    break;
                }
            }
        }
    }


    private bool CanShoot()
    {
        return !IsRangeAttacking && !IsMeleeAttacking && ShootingItem.Amount > 0;
    }

    public override void RightMouseHold(InputAction.CallbackContext context)
    {
        if (context.performed && !isrightClickPerformed) StartAim();
        else if (context.canceled || context.performed) EndAim();
    }
    public void EquipGun()
    {
        IsRaightClickHold = true;
    }
    public void StartAim()
    {
        isrightClickPerformed = true;
        Debug.Log("<color=green>Gun is Equiped</color>");
        animator.SetTrigger("Enable Gun");
        LeftPalm.SetCategoryAndLabel("L PALM", "Gun");
    }
    public void EndAim()
    {
        isrightClickPerformed = false;
        Debug.Log("<color=red>Gun is Unequiped</color>");
        IsRaightClickHold = false;
        animator.SetTrigger("Remove Gun");
        LeftPalm.SetCategoryAndLabel("L PALM", "Empty");
    }
    public void LateUpdate()
    {
        animator.SetBool("IsGunEquiped", IsRaightClickHold);
        if (IsRaightClickHold)
        {

            var worldPos = ConvertMousePosToWorldCoordinates(Input.mousePosition);
            GunIKTarget.position = worldPos;
            SetRotationTo(worldPos - transform.position);
        }
        GunIKTarget.position = Vector3.Lerp(LastIKPosition, GunIKTarget.position, 0.25f);
        GunArmSolver.UpdateIK(1);
        LastIKPosition = GunIKTarget.position;
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
