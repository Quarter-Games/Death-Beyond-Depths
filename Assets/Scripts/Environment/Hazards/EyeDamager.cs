using UnityEngine;
using DG.Tweening;
public class EyeDamager : MonoBehaviour
{
    [SerializeField] readonly float TimeToOpen = 1f;
    [SerializeField] readonly float TimeToClose = 1f;
    [SerializeField] readonly float TimeStaysOpen = 1f;
    [SerializeField] readonly float TimeStaysClosed = 2f;

    EyeState CurrentEyeState = EyeState.Closed;
    bool IsPlayerInTrigger = false;
    float timer = 0;

    static int NumberOfEyesSeeingPlayer = 0;

    private void Update()
    {
        if (!IsPlayerInTrigger)
        {
            CurrentEyeState = EyeState.Closed; 
            return;
        }
        timer += Time.deltaTime;
        switch (CurrentEyeState)
        {
            case EyeState.Closed:
                AdvanceState(TimeStaysClosed);
                break;
            case EyeState.Opening:
                break;
            case EyeState.Open:
                break;
            case EyeState.Closing:
                break;
            default:
                break;
        }
    }

    private void AdvanceState(float time)
    {
        if (timer >= time)
        {
            // Advance to next state, looping back to Closed after Closing
            CurrentEyeState = (EyeState)(((int)CurrentEyeState + 1) % System.Enum.GetValues(typeof(EyeState)).Length);
            timer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponent<PlayerCharacterController>())
        {
            return;
        }
        NumberOfEyesSeeingPlayer++;
        if (NumberOfEyesSeeingPlayer == 1)
        {
            DamageVignetteController.SpecialEffects.ScreenDamageOverTimeEffect();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.GetComponent<PlayerCharacterController>())
        {
            return;
        }
        NumberOfEyesSeeingPlayer--;
        if (NumberOfEyesSeeingPlayer <= 0)
        {
            NumberOfEyesSeeingPlayer = 0;
            DamageVignetteController.SpecialEffects.StopScreenDamageOverTimeEffect();
        }
    }

    public void StartEyeHazard()
    {
        IsPlayerInTrigger = true;
        CurrentEyeState = EyeState.Closed;
        timer = 0;
    }

    public void StopEyeHazard()
    {
        IsPlayerInTrigger = false;
        CurrentEyeState = EyeState.Closed;
        timer = 0;
    }
}

enum EyeState
{
    Closed,
    Opening,
    Open,
    Closing
}
