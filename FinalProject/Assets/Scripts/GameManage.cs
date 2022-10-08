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

            if(hit.collider != null)
            {
                if(curTarget != null)
                {
                    curTarget.DeSelect();
                }
                curTarget = hit.collider.GetComponent<NPC>();

                player.myTarget = curTarget.Select();

                //if(hit.collider.tag == "Enemy")
                //{
                //    player.myTarget = hit.transform.GetChild(0);
                //}
            }
            else
            {
                if(curTarget != null)
                {
                    curTarget.DeSelect();
                }
                curTarget = null;
                player.myTarget = null;
            }
        }
    }
}
