using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Enemy parent;
    private float attackCD = 2;
    private float extraRange = .1f;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
        parent.MyRb.velocity = Vector2.zero;
        parent.Direction = Vector2.zero;
    }

    public void Exit()
    {
        
    }


    void IState.Update()
    {
        if (parent.EnemyAttTime >= attackCD && !parent.IsAttacking) //make sure that enemy only attack when off cooldown and not in attacking
        {
            parent.EnemyAttTime = 0; //reset the attack timer
            parent.StartCoroutine(Attack()); //start the attack
        }

        if(parent.myTarget != null) //If have the target then check if can attack or if need to follow it
        {
            //calculates the distance between the target and the enemy
            float distance = Vector2.Distance(parent.myTarget.transform.position, parent.transform.parent.position);
            if(distance >= parent.EnemyAttRange+extraRange && !parent.IsAttacking) //if the distance is larger than the attackrange, then need to move
            {
                if(parent is RangedEnemy)
                {
                    parent.ChangeState(new PathState());
                }
                else
                {
                    parent.ChangeState(new FollowState());
                }
                //Follow the target
                
            }
        }
        else //if lost the target, then back to idle
        {
            parent.ChangeState(new IdleState());
        }
    }
    public IEnumerator Attack()
    {
        parent.IsAttacking = true;
        parent.MyAnim.SetTrigger("attack");
        //Vector3 dir = parent.myTarget.position - parent.transform.position;
        //parent.MyRb.velocity = dir.normalized * 0.5f;
        //parent.transform.parent.position = Vector2.MoveTowards(parent.transform.parent.position, dir, 2f * Time.deltaTime);
        yield return new WaitForSeconds(parent.MyAnim.GetCurrentAnimatorStateInfo(2).length);
        parent.IsAttacking = false;
    }
}
