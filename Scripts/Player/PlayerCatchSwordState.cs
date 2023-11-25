using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;
    public PlayerCatchSwordState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        sword = player.sword.transform;
        if (player.transform.position.x > sword.position.x && player.facingDir == 1)
            player.Flip();
        else if (player.transform.position.x < sword.position.x && player.facingDir == -1)
            player.Flip();
        rb.velocity = new Vector2(SkillManager.instance.sword.SwordReturnImpact * -player.facingDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        //player.StartCoroutine("BusyFor", .1f);//在回收剑的时候让人物停一下，但这是否有必要？
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            
            stateMachine.ChangeState(player.idleState);
        }
    }
}
