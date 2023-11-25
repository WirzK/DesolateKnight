using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJump);
            return;//否则会执行往下的语句，导致没有实际得到想要的效果，但为什么？也许这并非是一个中断？
        }

        if (yInput < 0)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        else
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .7f);

        if (xInput != 0 && xInput != player.facingDir)
            stateMachine.ChangeState(player.idleState);
        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);

        if (!player.IsWallDetected())
            stateMachine.ChangeState(player.airState);//不碰墙了就下落
    }
}
