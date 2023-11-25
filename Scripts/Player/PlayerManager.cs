using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player;//可在任意位置通过PlayerManager访问player
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);//确保单例
        else
            instance = this;
    }

}
