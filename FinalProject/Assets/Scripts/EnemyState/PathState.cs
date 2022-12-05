using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathState : IState
{
    //private Stack<Vector3> path;
    private Vector3 destination;
    private Vector3 current;
    private Vector3 goal;
    private Transform transform;
    private Enemy parent;
    private Vector3 targetPos;
    public void Enter(Enemy parent)
    {
        this.parent = parent;
        this.transform = parent.transform.parent; //(EnemyParent object)
        targetPos = Player.MyInstance.myCurrentTile.position; //will be used to keep check player current tile position
        if(targetPos != parent.myCurrentTile.position) //if the player current tile is different with enemy current tile
        {
            parent.MyPath = parent.EneAstar.Algorithm(parent.myCurrentTile.position, targetPos);
        }
        if(parent.MyPath != null)
        {
            Debug.Log("BEFORE = " + "current: " + current + ", destination: " + destination);
            //if((current != null) || (destination != null))
            //{
            //    current = parent.MyPath.Pop();
            //    destination = parent.MyPath.Pop();
            //}
            current = parent.MyPath.Pop();
            destination = parent.MyPath.Pop();
            //this.goal = parent.myCurrentTile.position;
            Debug.Log("AFTER = " + "current: " + current + ", destination: " + destination);
            //Debug.Log(parent.MyPath.Peek());
        }
        else
        {
            parent.ChangeState(new EvadeState());
        }
        if (!parent.inRange) //if Player is not in enemy range, then back to evade state/back to start position
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
        if(parent.MyPath != null)
        {
            //parent.ActivateLayers("Walk_Layer");
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

            //if (cur.y > dest.y)
            //{
            //    parent.Direction = Vector2.down;
            //}
            //else if(cur.y < dest.y)
            //{
            //    parent.Direction = Vector2.up;
            //}
            //if (cur.y == dest.y)
            //{
            //    if(cur.x > dest.x)
            //    {
            //        parent.Direction = Vector2.left;
            //    }
            //    else if(cur.x < dest.x)
            //    {
            //        parent.Direction = Vector2.right;
            //    }
            //}
            if(totalDistance <= parent.EnemyAttRange)
            {
                parent.ChangeState(new AttackState());
            }
            else if (Player.MyInstance.myCurrentTile.position == parent.myCurrentTile.position) //if enemy and player stand in same tile
            {
                parent.ChangeState(new FollowState()); //then start follow
            }
            if(distance <= 0f)
            {
                if(parent.MyPath.Count > 0)
                {
                    current = destination;
                    destination = parent.MyPath.Pop();

                    if(targetPos != Player.MyInstance.myCurrentTile.position) //if target position is different with the player position, then the player has moved to other tile
                    {
                        parent.ChangeState(new PathState());
                    }
                }
                else
                {
                    parent.MyPath = null;
                    parent.ChangeState(new PathState()); //for in case for weird situation like player teleported or other cases
                }
            }
        }
    }
}
