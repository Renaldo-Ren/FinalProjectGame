using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManage : MonoBehaviour
{
    [SerializeField]
    private Button[] actButton;

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
        if (Input.GetKeyDown(skill1))
        {
            ActionButtonClicked(0);
        }
        if (Input.GetKeyDown(skill2))
        {
            ActionButtonClicked(1);
        }
        if (Input.GetKeyDown(skill3))
        {
            ActionButtonClicked(2);
        }
    }

    private void ActionButtonClicked(int btnIndex)
    {
        actButton[btnIndex].onClick.Invoke();
    }
}
