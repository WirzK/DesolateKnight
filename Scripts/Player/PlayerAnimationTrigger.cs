using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private void AnimationTrigger()
    {
        player.AnimationTrigger();//ͨ��������ö���player�ڵ�animationtrigger����
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackChecks.position, player.attackCheckRadius);//�����һ֡�ڳ�����Ȧ�ڵ�������ײ��
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)//�е���
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
