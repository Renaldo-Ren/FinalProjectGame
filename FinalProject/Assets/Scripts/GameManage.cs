using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManage : MonoBehaviour
{
    private static GameManage instance;
    [SerializeField]
    public Player player;

    public NPC curTarget;

    private HashSet<Vector3Int> blocked = new HashSet<Vector3Int>();
    private int targetIndex = 0;

    public static GameManage MyInstance 
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GameManage>();
            }
            return instance;
        } 
    }
    public HashSet<Vector3Int> Blocked
    {
        get
        {
            return blocked;
        }
        set
        {
            blocked = value;
        }
    }

    void Update()
    {
        ClickTarget();
        NextTarget();
    }

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 256);

            if(hit.collider != null && hit.collider.tag == "Enemy")
            {
                DeSelectTarget();
                SelectTarget(hit.collider.GetComponent<Enemy>());
            }
            else 
            {
                UIManage.myInstance.HideTargetFrame();
                DeSelectTarget();
                curTarget = null;
                player.myTarget = null;
            }
        }
    }
    private void NextTarget()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            DeSelectTarget();
            if (Player.MyInstance.Attackers.Count > 0)
            {
                SelectTarget(Player.MyInstance.Attackers[targetIndex] as Enemy);
                targetIndex++;
                if (targetIndex >= Player.MyInstance.Attackers.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }
    public void DeSelectTarget()
    {
        if (curTarget != null)
        {
            curTarget.DeSelect();
        }
    }
    private void SelectTarget(Enemy enemy)
    {
        curTarget = enemy; 
        player.myTarget = curTarget.Select(); 
        UIManage.myInstance.ShowTargetFrame(curTarget);
    }
}
