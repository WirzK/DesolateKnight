using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public Dash_Skill dash { get; private set; }
    public Clone_Skill clone { get; private set; }
    public Sword_Skill sword { get; private set; }
    public void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
    public void Start()
    {
        dash = GetComponent<Dash_Skill>();//获取脚本，获得对应方法
        clone = GetComponent<Clone_Skill>();
        sword = GetComponent<Sword_Skill>();
    }
}
