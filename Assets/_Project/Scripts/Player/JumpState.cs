using UnityEngine;

public class JumpState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.Rb.linearVelocity = new Vector2(
            player.Rb.linearVelocity.x, player.JumpForce);
        // player.Animator.Play("Jump");
    }

    public void Update(PlayerController player)
    {
        if (player.DashPressed && player.CanDash)
        {
            player.StateMachine.ChangeState(new DashState());
            return;
        }

        // Ako padamo prema dolje i dotaknemo tlo — nazad u Idle ili Run
        if (player.Rb.linearVelocity.y <= 0 && player.IsGrounded)
        {
            if (player.MoveInput.x != 0)
                player.StateMachine.ChangeState(new RunState());
            else
                player.StateMachine.ChangeState(new IdleState());
            return;
        }

        if (player.AttackPressed && player.CanAttack)
        {
            player.StateMachine.ChangeState(new LightAttackState());
            return;
        }

        // Horizontalno kretanje u zraku
    }

    public void FixedUpdate(PlayerController player)
    {
        player.Rb.linearVelocity = new Vector2(
            player.MoveInput.x * player.MoveSpeed,
            player.Rb.linearVelocity.y);
    }

    public void Exit(PlayerController player) { }
}
