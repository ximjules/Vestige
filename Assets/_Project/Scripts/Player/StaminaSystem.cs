using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float MaxStamina = 100f;
    public float RegenDelay = 0.8f;
    public float RegenRate = 50f;

    [Header("Stamina Costs")]
    public float DashCost = 20f;
    public float LightAttackCost = 15f;
    public float HeavyAttackCost = 30f;

    private float _currentStamina;
    private float _regenTimer;

    public bool IsExhausted => _currentStamina <= 0f;

    // Events
    public event System.Action<float, float> OnStaminaChanged; // (current, max)

    private void Awake()
    {
        _currentStamina = MaxStamina;
    }

    private void Update()
    {
        if (_regenTimer > 0f)
        {
            _regenTimer -= Time.deltaTime;
            return;
        }

        if (_currentStamina >= MaxStamina) return;

        _currentStamina = Mathf.Min(
            _currentStamina + RegenRate * Time.deltaTime, MaxStamina);

        OnStaminaChanged?.Invoke(_currentStamina, MaxStamina);
    }

    public bool TryConsume(float amount)
    {
        if (_currentStamina < amount) return false;

        _currentStamina -= amount;
        _regenTimer = RegenDelay;

        OnStaminaChanged?.Invoke(_currentStamina, MaxStamina);
        return true;
    }

    public float CurrentStamina => _currentStamina;
}