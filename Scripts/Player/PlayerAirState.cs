using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlide);
        if (player.IsGroundDetected()) 
            stateMachine.ChangeState(player.idleState);
        if (xInput != 0)
            player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);//在空中变向

        
    }
}
