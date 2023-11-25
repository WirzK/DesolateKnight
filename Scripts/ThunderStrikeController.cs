 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float speed;
    private Animator anim;
    private bool triggered;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();

    }
     
    void Update()
    {
        if (triggered)
            return;

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;//调整闪电朝向

        if(Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            anim.transform.localPosition = new Vector3(0, .5f);//调整闪电位置
            anim.transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);

            Invoke("DamageAndSelfDestroy", .2f);
            triggered = true;
            anim.SetTrigger("Hit");
        }
        
    }

    private void DamageAndSelfDestroy()
    {
        targetStats.TakeDamage(1);//造成伤害   
        Destroy(gameObject, .4f);//0.4s后摧毁

    }
}
