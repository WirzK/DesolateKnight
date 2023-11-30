using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject character;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject skillTree;
    bool CanOpen = true;
    // Start is called before the first frame update
    void Start()
    {
        SwitchTo(inGameUI);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && CanOpen)
        {
            SwitchTo(character);
            CanOpen = false;
            return;

        }
        if (Input.GetKeyDown(KeyCode.C) && !CanOpen)
        {
            CanOpen = true;
            SwitchTo(character);
            character.SetActive(false);
            CheckForInGameUI();
            return;
        }
        

    }


    public void SwitchTo(GameObject _menu)
    {
        
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);

        }
        if (_menu != null)
            _menu.SetActive(true);

    }

    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
                return;
        }
        SwitchTo(inGameUI);//什么都没开则显示UI
    }


}
