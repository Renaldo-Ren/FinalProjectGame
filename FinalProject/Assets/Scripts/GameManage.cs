using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManage : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private NPC curTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ClickTarget();
    }

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 256);

            if(hit.collider != null) //if we hit something
            {
                if(curTarget != null) //if we have current target
                {
                    curTarget.DeSelect(); //deselect the current target
                }
                curTarget = hit.collider.GetComponent<NPC>(); //selects the new target

                player.myTarget = curTarget.Select(); //gives the player the new target

                UIManage.myInstance.ShowTargetFrame(curTarget);
                //if(hit.collider.tag == "Enemy")
                //{
                //    player.myTarget = hit.transform.GetChild(0);
                //}
            }
            else //deselect the target
            {
                UIManage.myInstance.HideTargetFrame();
                if(curTarget != null) //if we have a current target
                {
                    curTarget.DeSelect(); //we deselect it
                }
                curTarget = null;
                player.myTarget = null;
            }
        }
    }
}
