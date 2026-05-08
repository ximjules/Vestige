using UnityEngine;

public class LightAttackState : IPlayerState
{
    private float _attackTimer;
    private bool _hasDealtDamage;

    public void Enter(PlayerController player)
    {
        if (!player.Stamina.TryConsume(player.Stamina.LightAttackCost))
        {
            player.StateMachine.ChangeState(new IdleState());
            return;
        }

        _attackTimer = player.LightAttackCooldown;
        _hasDealtDamage = false;

        player.Rb.linearVelocity = new Vector2(0f, player.Rb.linearVelocity.y);
        player.StartAttackCooldown();
        // player.Animator.Play("LightAttack");
    }

    public void Update(PlayerController player)
    {
        _attackTimer -= Time.deltaTime;

        // Dealt damage na sredini animacije (frame 0.1s)
        if (!_hasDealtDamage && _attackTimer <= player.LightAttackCooldown * 0.6f)
        {
            DealDamage(player);
            _hasDealtDamage = true;
        }

        if (_attackTimer <= 0f)
        {
            if (player.MoveInput.x != 0 && player.IsGrounded)
                player.StateMachine.ChangeState(new RunState());
            else
                player.StateMachine.ChangeState(new IdleState());
        }
    }

    private void DealDamage(PlayerController player)
    {
        Vector2 attackPos = (Vector2)player.transform.position +
                            new Vector2(player.IsFacingRight ? 1f : -1f, 0f);

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPos, player.LightAttackRange, player.EnemyLayer);

        foreach (Collider2D hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable == null || !damageable.IsAlive) continue;

            Vector2 knockbackDir = new Vector2(
                player.IsFacingRight ? 1f : -1f, 0.3f).normalized;

            damageable.TakeDamage(
                player.LightAttackDamage, knockbackDir, 8f);
        }

        player.GetComponent<RallySystem>()?.TryConsumeRally();
    }

    public void FixedUpdate(PlayerController player) { }

    public void Exit(PlayerController player) { }
}