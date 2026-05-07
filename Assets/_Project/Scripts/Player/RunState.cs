using UnityEngine;

public class RunState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        // player.Animator.Play("Run");
    }

    public void Update(PlayerController player)
    {
        if (player.MoveInput.x == 0 && player.IsGrounded)
        {
            player.StateMachine.ChangeState(new IdleState());
            return;
        }

        if (player.JumpPressed && player.IsGrounded)
        {
            player.StateMachine.ChangeState(new JumpState());
            return;
        }

        if (player.DashPressed && player.CanDash)
        {
            player.StateMachine.ChangeState(new DashState());
            return;
        }

        if (player.AttackPressed && player.CanAttack)
        {
            player.StateMachine.ChangeState(new LightAttackState());
            return;
        }
    }

    public void FixedUpdate(PlayerController player)
    {
        player.Rb.linearVelocity = new Vector2(
            player.MoveInput.x * player.MoveSpeed,
            player.Rb.linearVelocity.y);
    }

    public void Exit(PlayerController player) { }
}
