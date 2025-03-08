using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField, Min(0)] float MaxStamina = 100f;
    [SerializeField, Min(0)] float MinStamina = 25f;
    [SerializeField, Min(0)] float StaminaRestorationSpeed = 0.5f;
    [SerializeField, Min(0)] float _StaminaThreshold;

    float _currentStamina;

    public event Action OnStaminaChanged;

    public float CurrentStamina
    {
        get
        {
            return Mathf.Clamp(_currentStamina, 0, MaxStamina);
        }
        private set { _currentStamina = value; }
    }

    public float StaminaThreshold
    {
        get
        {
            return Mathf.Clamp(_StaminaThreshold, MinStamina, MaxStamina);
        }
        private set { _StaminaThreshold = value; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StaminaThreshold = MaxStamina;
        CurrentStamina = MaxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        RestoreStaminaToThreshold();
    }

    public void ConsumeStamina(float amount)
    {
        if (_currentStamina < amount) return; //not enough stamina
        CurrentStamina -= amount;
        StaminaThreshold -= amount / 2;
        OnStaminaChanged?.Invoke();
    }

    public void RestoreStaminaToThreshold()
    {
        if (CurrentStamina >= StaminaThreshold) return;
        Mathf.Lerp(CurrentStamina, StaminaThreshold, StaminaRestorationSpeed);
        OnStaminaChanged?.Invoke();
    }

    public void RestoreThreshold(float amount)
    {
        StaminaThreshold += amount;
    }
}
