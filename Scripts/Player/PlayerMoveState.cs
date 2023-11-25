using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
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
        //if (player.IsWallDetected())
        //    stateMachine.ChangeState(player.idleState);//没必要吧，站墙边就没法走...

        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        if (xInput == 0){
            stateMachine.ChangeState(player.idleState);
        }
    }
}
