using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField, Min(0)] float MaxStamina = 100f;
    [SerializeField, Min(0)] float MinStamina = 25f;
    [SerializeField, Min(0)] float StaminaThreshold;

    float _currentStamina;

    public float CurrentStamina 
    {
        get 
        {
            return Mathf.Clamp(_currentStamina, 0, MaxStamina); 
        }
        private set { _currentStamina = value; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentStamina = MaxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
