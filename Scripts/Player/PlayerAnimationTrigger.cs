using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private void AnimationTrigger()
    {
        player.AnimationTrigger();//通过本类调用对象player内的animationtrigger方法
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackChecks.position, player.attackCheckRadius);//检测这一帧内出现在圈内的所有碰撞器
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)//有敌人
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                player.stats.DoDamage(_target); 


            }
            
        }
    }   
    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }

}
