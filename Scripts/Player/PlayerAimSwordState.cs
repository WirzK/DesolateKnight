using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.skill.sword.DotsActive(true);//开始制剑，启动辅助线
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .2f);//发射剑的时候让人物停一下，但这是否有必要？
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();//这将在发射时禁止移动
        
        if (Input.GetKeyUp(KeyCode.Mouse1))
            stateMachine.ChangeState(player.idleState);

        Vector2 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (player.transform.position.x > MousePosition.x && player.facingDir == 1)
            player.Flip();
        else if (player.transform.position.x < MousePosition.x && player.facingDir == -1)
            player.Flip();
    }
}
