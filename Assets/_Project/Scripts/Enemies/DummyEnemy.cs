using UnityEngine;

public class DummyEnemy : MonoBehaviour, IDamageable
{
    public float MaxHP = 50f;
    public float AttackDamage = 20f;
    public float AttackRange = 1.5f;
    public float AttackCooldown = 2f;

    private float _currentHP;
    private float _attackTimer = 0f;
    private PlayerHealth _playerHealth;

    public bool IsAlive => _currentHP > 0f;

    private void Awake()
    {
        _currentHP = MaxHP;
        _playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void Update()
    {
        if (!IsAlive || _playerHealth == null) return;

        _attackTimer -= Time.deltaTime;

        float distToPlayer = Vector2.Distance(
            transform.position, _playerHealth.transform.position);

        if (distToPlayer <= AttackRange && _attackTimer <= 0f)
        {
            Vector2 knockbackDir = (_playerHealth.transform.position
                                  - transform.position).normalized;

            _playerHealth.TakeDamage(AttackDamage, knockbackDir, 5f);
            _attackTimer = AttackCooldown;
        }
    }

    public void TakeDamage(float amount, Vector2 knockbackDir, float knockbackForce)
    {
        if (!IsAlive) return;
        _currentHP -= amount;
        Debug.Log($"Enemy hit! Damage: {amount} | HP remaining: {_currentHP}");
        if (!IsAlive) Debug.Log("Enemy defeated!");
    }
}
