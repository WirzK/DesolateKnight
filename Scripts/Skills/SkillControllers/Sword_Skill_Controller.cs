using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 12;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;
    private bool canRotate = true;
    private bool isReturning;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        
        cd = GetComponent<CircleCollider2D>();

    }
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        cd = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetUpSword(Vector2 _dir, float _gravityScale, Player _player)
    {
        player = _player;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
        anim.SetBool("Rotation", true);
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;

    }
    public void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            anim.SetBool("Rotation", true);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchTheSword();

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;
        anim.SetBool("Rotation", false);
        canRotate = false;
        cd.enabled = false;// 关闭碰撞器

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;//禁止在任何轴上的旋转
        transform.parent = collision.transform;//切换父对象

    }
}
