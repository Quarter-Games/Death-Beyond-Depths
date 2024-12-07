using UnityEngine;

public class GirlController : PlayerCharacterController
{
    [SerializeField, Range(0, 1)] float CrouchSpeedModifier;
    [SerializeField] GameObject StandingCharacterObject;
    [SerializeField] GameObject CrouchingCharacterObject;
    [SerializeField] Collider2D StandingCollider;
    [SerializeField] Collider2D CrouchingCollider;

    bool isStanding;

    protected override void SpecialMove()
    {
        isStanding = StandingCharacterObject.activeSelf;
        ToggleStandingCrouching();
        stats.MovementSpeed *= isStanding ? CrouchSpeedModifier : (1 / CrouchSpeedModifier);
    }

    private void ToggleStandingCrouching()
    {
        StandingCharacterObject.SetActive(!isStanding);
        StandingCollider.enabled = !isStanding;
        CrouchingCharacterObject.SetActive(isStanding);
        CrouchingCollider.enabled = isStanding;
    }
}
