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
        //player.anim.speed = 1.2f;//�ӿ춯�������ٶ� 
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
        //player.anim.speed = 1;//���������ٶ�
        player.StartCoroutine("BusyFor", .1f);//���ٻ�������ͬʱ����ʱ�����ƶ����д��Ż�

        comboCounter++;
        lastTimeAttacked = Time.time;


    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            player.SetZeroVelocity();//����ʱͣ�£������Ƿ�����б�Ҫ��

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    
}
