using UnityEngine;

public class ShamblerAI : EnemyStateMachine
{
    private enum State { Patrol, Chase, Attack, Stagger, Dead }
    private State _currentState = State.Patrol;

    [Header("Patrol")]
    public float PatrolDistance = 3f;
    public float PatrolWaitTime = 1.5f;

    private Vector3 _startPosition;
    private float _patrolTimer;
    private int _patrolDirection = 1;
    private float _attackCooldown = 0f;

    protected override void Awake()
    {
        base.Awake();
        _startPosition = transform.position;
    }

    private void Update()
    {
        if (!IsAlive) return;
        _attackCooldown -= Time.deltaTime;

        switch (_currentState)
        {
            case State.Patrol: UpdatePatrol(); break;
            case State.Chase: UpdateChase(); break;
            case State.Attack: UpdateAttack(); break;
        }
    }

    private void UpdatePatrol()
    {
        if (_player != null)
        {
            float dist = Vector2.Distance(transform.position, _player.position);
            if (dist <= DetectionRange)
            {
                _currentState = State.Chase;
                return;
            }
        }

        // Hodaj lijevo-desno oko start pozicije
        _rb.linearVelocity = new Vector2(_patrolDirection * MoveSpeed, _rb.linearVelocity.y);
        FlipTowards(transform.position + Vector3.right * _patrolDirection);

        float distFromStart = transform.position.x - _startPosition.x;
        if (Mathf.Abs(distFromStart) >= PatrolDistance)
        {
            _patrolDirection *= -1;
            _patrolTimer = PatrolWaitTime;
            _rb.linearVelocity = Vector2.zero;
        }
    }

    private void UpdateChase()
    {
        if (_player == null) return;

        float dist = Vector2.Distance(transform.position, _player.position);

        // Izgubio playera
        if (dist > DetectionRange * 1.5f)
        {
            _currentState = State.Patrol;
            return;
        }

        // Dovoljno blizu za napad
        if (dist <= AttackRange)
        {
            _currentState = State.Attack;
            return;
        }

        // Juri prema playeru
        float dir = _player.position.x > transform.position.x ? 1f : -1f;
        _rb.linearVelocity = new Vector2(dir * ChaseSpeed, _rb.linearVelocity.y);
        FlipTowards(_player.position);
    }

    private void UpdateAttack()
    {
        if (_player == null) return;

        _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);

        float dist = Vector2.Distance(transform.position, _player.position);

        // Player izašao iz attack rangea
        if (dist > AttackRange)
        {
            _currentState = State.Chase;
            return;
        }

        // Napadni ako cooldown istekao
        if (_attackCooldown <= 0f)
        {
            PlayerHealth playerHealth = _player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Vector2 knockbackDir = (_player.position - transform.position).normalized;
                playerHealth.TakeDamage(AttackDamage, knockbackDir, 5f);
            }
            _attackCooldown = 2f;
        }
    }

    public override void TakeDamage(float amount, Vector2 knockbackDir, float knockbackForce)
    {
        if (!IsAlive) return;
        _currentState = State.Stagger;
        base.TakeDamage(amount, knockbackDir, knockbackForce);
        StartCoroutine(RecoverFromStagger());
    }

    private System.Collections.IEnumerator RecoverFromStagger()
    {
        yield return new WaitForSeconds(StaggerDuration);
        if (IsAlive) _currentState = State.Chase;
    }

    protected override void Die()
    {
        _currentState = State.Dead;
        _rb.linearVelocity = Vector2.zero;
        base.Die();
    }
}
