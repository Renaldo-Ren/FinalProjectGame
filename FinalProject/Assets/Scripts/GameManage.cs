using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManage : MonoBehaviour
{
    private static GameManage instance;
    [SerializeField]
    private Player player;

    private NPC curTarget;

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

    // Start is called before the first frame update
    void Start()
    {
        //Player.MyInstance.SetDefaultValues();
    }

    // Update is called once per frame
    void Update()
    {
        ClickTarget();
        NextTarget();
    }

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            //Makes a raycast from the mouse position into the game world
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 256);

            if(hit.collider != null && hit.collider.tag == "Enemy") //if we hit something
            {
                DeSelectTarget();
                SelectTarget(hit.collider.GetComponent<Enemy>());
                //if(hit.collider.tag == "Enemy")
                //{
                //    player.myTarget = hit.transform.GetChild(0);
                //}
            }
            else //deselect the target
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
            if (Player.MyInstance.MyAttackers.Count > 0)
            {
                SelectTarget(Player.MyInstance.MyAttackers[targetIndex]);
                targetIndex++;
                if(targetIndex >= Player.MyInstance.MyAttackers.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }
    private void DeSelectTarget()
    {
        if (curTarget != null) //if we have current target
        {
            curTarget.DeSelect(); //deselect the current target
        }
    }
    private void SelectTarget(Enemy enemy)
    {
        curTarget = enemy; //selects the new target
        player.myTarget = curTarget.Select(); //gives the player the new target
        UIManage.myInstance.ShowTargetFrame(curTarget);
    }
    public void OnKillConfirmed(Character character)
    {
        Destroy(character.transform.GetChild(0).gameObject);
    }
}
