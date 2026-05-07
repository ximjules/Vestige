using UnityEngine;

public class DashState : IPlayerState
{
    private float _dashTimer;
    private float _dashDirection;

    public void Enter(PlayerController player)
    {
        _dashTimer = player.DashDuration;
        _dashDirection = player.IsFacingRight ? 1f : -1f;

        player.Rb.gravityScale = 0f;
        player.Rb.linearVelocity = new Vector2(
            _dashDirection * player.DashForce, 0f);

        player.StartDashCooldown();
        // player.Animator.Play("Dash");
    }

    public void Update(PlayerController player)
    {
        _dashTimer -= Time.deltaTime;

        if (_dashTimer <= 0f)
        {
            player.Rb.gravityScale = 3f;

            if (player.MoveInput.x != 0)
                player.StateMachine.ChangeState(new RunState());
            else
                player.StateMachine.ChangeState(new IdleState());
        }
    }

    public void FixedUpdate(PlayerController player) { }

    public void Exit(PlayerController player)
    {
        player.Rb.gravityScale = 3f;
    }
}