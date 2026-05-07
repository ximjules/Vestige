using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float amount, Vector2 knockbackDir, float knockbackForce);
    bool IsAlive { get; }
}
