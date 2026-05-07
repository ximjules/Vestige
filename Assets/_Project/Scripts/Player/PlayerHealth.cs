using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public float MaxHP = 100f;

    private float _currentHP;
    private RallySystem _rally;

    public bool IsAlive => _currentHP > 0f;

    // Events
    public event System.Action<float, float> OnHealthChanged; // (current, max)
    public event System.Action OnDied;

    private void Awake()
    {
        _currentHP = MaxHP;
        _rally = GetComponent<RallySystem>();
    }

    public void TakeDamage(float amount, Vector2 knockbackDir, float knockbackForce)
    {
        if (!IsAlive) return;

        _currentHP -= amount;
        _currentHP = Mathf.Max(_currentHP, 0f);

        _rally?.OnDamageReceived(amount);

        Debug.Log($"Player hit! HP: {_currentHP}/{MaxHP}");
        OnHealthChanged?.Invoke(_currentHP, MaxHP);

        if (_currentHP <= 0f)
        {
            OnDied?.Invoke();
            Debug.Log("Player died!");
        }
    }

    public void Heal(float amount)
    {
        if (!IsAlive) return;
        _currentHP = Mathf.Min(_currentHP + amount, MaxHP);
        OnHealthChanged?.Invoke(_currentHP, MaxHP);
    }
}
