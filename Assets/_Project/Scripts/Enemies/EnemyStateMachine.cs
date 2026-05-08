using UnityEngine;

public class EnemyStateMachine : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public float MaxHP = 60f;
    public float MoveSpeed = 2f;
    public float ChaseSpeed = 4f;
    public float AttackDamage = 15f;
    public float AttackRange = 1.2f;
    public float DetectionRange = 6f;
    public float StaggerDuration = 0.4f;
    public int EchoesOnDeath = 30;

    protected float _currentHP;
    protected Transform _player;
    protected Rigidbody2D _rb;

    // State
    public bool IsAlive => _currentHP > 0f;
    public bool IsFacingRight { get; protected set; } = true;

    // Events
    public event System.Action OnDeath;

    protected virtual void Awake()
    {
        _currentHP = MaxHP;
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public virtual void TakeDamage(float amount, Vector2 knockbackDir, float knockbackForce)
    {
        if (!IsAlive) return;

        _currentHP -= amount;
        _rb.linearVelocity = knockbackDir * knockbackForce;

        Debug.Log($"{gameObject.name} hit! HP: {_currentHP}/{MaxHP}");

        if (_currentHP <= 0f)
        {
            Die();
            return;
        }

        StartCoroutine(StaggerRoutine());
    }

    protected virtual void Die()
    {
        EchoSystem.Instance?.AddEchoes(EchoesOnDeath);
        OnDeath?.Invoke();
        Debug.Log($"{gameObject.name} defeated! +{EchoesOnDeath} echoes");
        Destroy(gameObject);
    }

    protected System.Collections.IEnumerator StaggerRoutine()
    {
        yield return new WaitForSeconds(StaggerDuration);
    }

    protected void FlipTowards(Vector3 target)
    {
        bool shouldFaceRight = target.x > transform.position.x;
        if (shouldFaceRight == IsFacingRight) return;

        IsFacingRight = shouldFaceRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
