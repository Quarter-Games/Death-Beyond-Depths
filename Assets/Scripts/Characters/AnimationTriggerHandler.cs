using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AnimationTriggerHandler : MonoBehaviour
{
    [SerializeField] AudioResource StepClips;
    [SerializeField] AudioResource CrouchClip;
    [SerializeField] AudioResource LadderClips;
    [SerializeField] AudioResource DeathClip;
    [SerializeField] CaptainController captainController;
    public void OnStepEvent()
    {
        AudioManager.Instance.PlaySoundEffect(StepClips, transform);
    }
    public void OnCrouchEvent()
    {
        AudioManager.Instance.PlaySoundEffect(CrouchClip, transform);
    }
    public void OnLadderEvent()
    {
        AudioManager.Instance.PlaySoundEffect(LadderClips, transform);
    }
    public void OnDeathEvent()
    {
        AudioManager.Instance.PlaySoundEffect(DeathClip, transform);
    }
    public void OnShootEvent()
    {
        captainController.Shoot();
    }
    public void OnEquipGun()
    {
        captainController.EquipGun();
    }

    public void OnMeleeFinished()
    {
        captainController.StartAttackCooldown();
    }

    public void OnGetHitFinished()
    {
        captainController.OnStoppedGettingHit();
    }
    public void OnRespawnEnd()
    {
        captainController.CallEnableInput();
    }
}
