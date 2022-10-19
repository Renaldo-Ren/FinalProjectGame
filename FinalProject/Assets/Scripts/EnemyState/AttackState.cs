using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Enemy parent;
    private float attackCD = 3;
    private float extraRange = .1f;
    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
        
    }


    void IState.Update()
    {
        if (parent.EnemyAttTime >= attackCD && !parent.IsAttacking)
        {
            parent.EnemyAttTime = 0;
            parent.StartCoroutine(Attack());
        }

        if(parent.Target != null) //If have the target
        {
            //calculates the distance between the target and the enemy
            float distance = Vector2.Distance(parent.Target.position, parent.transform.position);
            if(distance >= parent.EnemyAttRange+extraRange && !parent.IsAttacking)
            {
                //Follow the target
                parent.ChangeState(new FollowState());
            }
            //...
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
        //Vector3 dir = parent.Target.position - parent.transform.position;
        //parent.MyRb.velocity = dir.normalized * 0.5f;
        //parent.transform.position = Vector2.MoveTowards(parent.transform.position, dir, parent.Speed * Time.deltaTime);
        yield return new WaitForSeconds(parent.MyAnim.GetCurrentAnimatorStateInfo(2).length);
        parent.IsAttacking = false;
    }
}
