using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField]protected LayerMask whatIsPlayer;

    [Header("Stunned Info")]
    public float stunDuration;//ѣ��ʱ��
    public Vector2 stunDirection;//ѣ�λ���
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    [Header("Move Info")]
    public float moveSpeed;
    public float idleTime;//��ǽ�󷢴���ʱ��
    public float battleTime;//����ս��״̬��ʱ��
    public float discoverDistance;//������ҵľ���
    public float giveUpDistance;//����޾���
    private float defaultMoveSpeed;

    [Header("Attack Info")]
    public float attackDistance;
    public float attackCoolDown;
    [HideInInspector] public float lastTimeAttacked;


    public EnemyStateMachine stateMachine { get; private set; }
    public string lastAnimBoolName { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }
    protected override void Start()
    {
        base.Start();
        defaultMoveSpeed = moveSpeed;
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        
    }
    public virtual void AssignLastAniName(string _animBoolName) => lastAnimBoolName = _animBoolName;


    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    public override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
    }

    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    } 

    public virtual bool CanBeStunned()
    {
        if(canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, discoverDistance, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }

}
