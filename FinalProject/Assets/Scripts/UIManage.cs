using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManage : MonoBehaviour
{
    [SerializeField]
    private Button[] actButton;

    //public bool CD1 = false;
    //public bool CD2 = false;
    //public bool CD3 = false;
    [SerializeField]
    private SkillSet skillSet;

    private KeyCode skill1, skill2, skill3;
    // Start is called before the first frame update
    void Start()
    {
        //Keybinds
        skill1 = KeyCode.Alpha1;
        skill2 = KeyCode.Alpha2;
        skill3 = KeyCode.Alpha3;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(skill1) && skillSet.CD1() == false)
        {
            ActionButtonClicked(0);
        }
        if (Input.GetKeyDown(skill2) && skillSet.CD2() == false)
        {
            ActionButtonClicked(1);
        }
        if (Input.GetKeyDown(skill3) && skillSet.CD3() == false)
        {
            ActionButtonClicked(2);
        }
    }

    private void ActionButtonClicked(int btnIndex)
    {
        actButton[btnIndex].onClick.Invoke();
    }
}
