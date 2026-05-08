using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("Health & Rally")]
    public Slider HealthBar;

    [Header("Stamina")]
    public Slider StaminaBar;

    [Header("Echoes")]
    public TextMeshProUGUI EchoText;

    private PlayerHealth _health;
    private StaminaSystem _stamina;

    private void Start()
    {
        _health = FindObjectOfType<PlayerHealth>();
        _stamina = FindObjectOfType<StaminaSystem>();

        if (_health != null)
        {
            _health.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(_health.MaxHP, _health.MaxHP);
        }

        if (_stamina != null)
        {
            _stamina.OnStaminaChanged += UpdateStaminaBar;
            UpdateStaminaBar(_stamina.MaxStamina, _stamina.MaxStamina);
        }

        if (EchoSystem.Instance != null)
            EchoSystem.Instance.OnEchoesChanged += UpdateEchoCounter;

        UpdateEchoCounter(0);
    }

    private void UpdateHealthBar(float current, float max)
    {
        if (HealthBar == null) return;
        HealthBar.value = current / max;
    }

    private void UpdateStaminaBar(float current, float max)
    {
        if (StaminaBar == null) return;
        StaminaBar.value = current / max;
    }

    private void UpdateEchoCounter(int amount)
    {
        if (EchoText == null) return;
        EchoText.text = $"⌘ {amount}";
    }
}