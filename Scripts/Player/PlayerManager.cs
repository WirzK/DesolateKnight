using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Player player;//��������λ��ͨ��PlayerManager����player
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);//ȷ������
        else
            instance = this;
    }

}
