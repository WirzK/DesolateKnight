using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LoseSkill : MonoBehaviour
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
    public void ChangeSkillToEnAble(GameObject _skill)
    {
        _skill.SetActive(false);
        parent.SetActive(true);
        return;
    }
}
