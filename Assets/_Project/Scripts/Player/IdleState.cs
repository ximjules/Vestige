using NUnit.Framework.Interfaces;
using UnityEngine;

public class IdleState : IPlayerState
{
    public void Enter(PlayerController player)
    {
        player.Rb.linearVelocity = new Vector2(0f, player.Rb.linearVelocity.y);
        // player.Animator.Play("Idle");
    }

    public void Update(PlayerController player)
    {
        if (player.MoveInput.x != 0)
        {
            player.StateMachine.ChangeState(new RunState());
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

    public void FixedUpdate(PlayerController player) { }

    public void Exit(PlayerController player) { }
}
