using UnityEngine;
using UnityEngine.Audio;

public class EnemyScreamEmitter : MonoBehaviour
{
    [SerializeField] ParticleSystem scream;
    [SerializeField] AudioResource screamAudio;
    [SerializeField] AudioResource GrowlSound;
    [SerializeField] AudioResource StepSound;
    [SerializeField] AudioResource RunSound;
    [SerializeField] Transform StepLeavingPos;
    public void EmitGrowl()
    {
        AudioManager.Instance.PlaySoundEffect(GrowlSound, transform);
    }
    public void EmitScream()
    {
        scream.Play();
        AudioManager.Instance.PlaySoundEffect(screamAudio, transform);
    }
    public void EmitStep()
    {
        AudioManager.Instance.PlaySoundEffect(StepSound, StepLeavingPos);
    }
    public void EmitRun()
    {
        AudioManager.Instance.PlaySoundEffect(RunSound, StepLeavingPos);
    }
}
