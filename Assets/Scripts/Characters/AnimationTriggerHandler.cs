using UnityEngine;

public class AnimationTriggerHandler : MonoBehaviour
{
    [SerializeField] CaptainController captainController;
    public void OnShootEvent()
    {
        captainController.Shoot();
    }
    public void OnEquipGun()
    {
        captainController.EquipGun();
    }
    public void OnBackStepAnimationEnd()
    {
        captainController.OnBackStepAnimationComplete();
    }
    public void OnBackStepForceAdd()
    {
        captainController.BackStepForce();
    }

    public void OnMeleeFinished()
    {
        captainController.StartAttackCooldown();
    }

    public void OnGetHitFinished()
    {
        captainController.OnStoppedGettingHit();
    }
}
