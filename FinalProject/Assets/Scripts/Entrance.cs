using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    public string entrancePassword;
    // Start is called before the first frame update
    private void Start()
    {
        if (Player.MyInstance.scenePassword == entrancePassword)
        {
            Player.MyInstance.transform.parent.position = transform.position;

            UIManage.myInstance.HideTargetFrame();
            GameManage.MyInstance.DeSelectTarget();
            GameManage.MyInstance.curTarget = null;
            GameManage.MyInstance.player.myTarget = null;
        }
        //else
        //{
        //    Debug.Log("Wrong PW");
        //}
    }
}
