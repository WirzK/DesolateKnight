using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GetSkill : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject child;
     
    // Start is called before the first frame update
    void Start()
    {
        
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeSkillToAble(GameObject _skill)
    {
        _skill.SetActive(false);
        child.SetActive(true);
        return;
    }
}
