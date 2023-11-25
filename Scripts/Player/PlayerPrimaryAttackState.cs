using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;
    private float lastTimeAttacked;
    private float comboWindow = .8f;


    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        xInput = 0;//we need this to fix a bug on attack direction
        if (comboCounter > 2 || Time.time > lastTimeAttacked + comboWindow)
            comboCounter = 0;
        player.anim.SetInteger("ComboCounter", comboCounter);
        //player.anim.speed = 1.2f;//加快动画播放速度 
        #region  Choose attack direction
        float attackDir = player.facingDir;

        if (xInput != 0)
            attackDir = xInput;

        #endregion
        attackDir = player.facingDir;
        if (xInput != 0)
            attackDir = xInput; 

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);
        stateTimer = .1f;
    }

    public override void Exit()

    {
        base.Exit();
        //player.anim.speed = 1;//返还动画速度
        player.StartCoroutine("BusyFor", .1f);//不再滑动，但同时攻击时不能移动，有待优化

        comboCounter++;
        lastTimeAttacked = Time.time;


    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            player.SetZeroVelocity();//攻击时停下，但这是否真的有必要？

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    
}
