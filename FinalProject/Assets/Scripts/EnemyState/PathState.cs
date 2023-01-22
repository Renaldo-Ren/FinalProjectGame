using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathState : IState
{
    private Vector3 destination;
    private Vector3 current;
    private Transform transform;
    private Enemy parent;
    private Vector3 targetPos;
    public void Enter(Enemy parent)
    {
        this.parent = parent;
        this.transform = parent.transform.parent;
        targetPos = Player.MyInstance.myCurrentTile.position;
        if(targetPos != parent.myCurrentTile.position)
        {
            parent.MyPath = parent.EneAstar.Algorithm(parent.myCurrentTile.position, targetPos);
        }
        if(parent.MyPath != null)
        {
            current = parent.MyPath.Pop();
            if(parent.MyPath.Count > 0)
            {
                destination = parent.MyPath.Pop();
            }
        }
        else
        {
            parent.ChangeState(new EvadeState());
        }
        if (!parent.inRange)
        {
            parent.ChangeState(new EvadeState());
        }
    }

    public void Exit()
    {
        parent.MyPath = null;
    }

    public void Update()
    {
        //Debug.Log("Target Pos: " + targetPos + ", destination: " + destination + ", current: " + current + ", transform: " + transform + ", parent: " + parent + ", parent path: " + parent.MyPath);
        if (parent.MyPath != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, 2 * Time.deltaTime);
            Vector3Int dest = parent.EneAstar.MyTilemap.WorldToCell(destination);
            Vector3Int cur = parent.EneAstar.MyTilemap.WorldToCell(current);
            float distance = Vector2.Distance(destination, transform.position);
            float totalDistance = Vector2.Distance(parent.myTarget.transform.position, parent.transform.position);
            if (cur.x > dest.x)
            {
                parent.Direction = Vector2.left;
            }
            else if (cur.x < dest.x)
            {
                parent.Direction = Vector2.right;
            }
            if(totalDistance <= parent.EnemyAttRange)
            {
                parent.ChangeState(new AttackState());
            }
            else if (Player.MyInstance.myCurrentTile.position == parent.myCurrentTile.position)
            {
                parent.ChangeState(new FollowState());
            }
            if(distance <= 0f)
            {
                if(parent.MyPath.Count > 0)
                {
                    current = destination;
                    destination = parent.MyPath.Pop();

                    if(targetPos != Player.MyInstance.myCurrentTile.position)
                    {
                        parent.ChangeState(new PathState());
                    }
                }
                else
                {
                    parent.MyPath = null;
                    parent.ChangeState(new PathState());
                }
            }
        }
    }
}
