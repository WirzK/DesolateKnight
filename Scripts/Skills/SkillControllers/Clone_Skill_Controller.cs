using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLosingSpeed;//Ӱ����ʧ���ٶ�
    
    private float cloneTimer;//��ʱ��
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = 1;
    private Transform closestEnemy;


    public void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        
    }
    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLosingSpeed));
            if (sr.color.a <= 0)
                Destroy(gameObject);//�ݻ�Ӱ��+
        }
        
        
    }

    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack)
    {
        if (_canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 4));
        transform.position = _newTransform.position;
        cloneTimer = _cloneDuration;

        FaceClosetTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f; //����Ϊ�������俪ʼ��ʧ

    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);//�����һ֡�ڳ�����Ȧ�ڵ�������ײ��
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)//�е���
                hit.GetComponent<Enemy>().DamageEffect();//����˺� 
        }
        
    }

    private void FaceClosetTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closeDistance = Mathf.Infinity;
        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if(distanceToEnemy < closeDistance)
                {
                    closeDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }

            }
        }

        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
                transform.Rotate(0, 180, 0);
        }

    }

}
