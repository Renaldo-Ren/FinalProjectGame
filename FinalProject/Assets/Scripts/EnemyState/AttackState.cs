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
        if (parent.EnemyAttTime >= attackCD && !parent.IsAttacking) 
        {
            parent.EnemyAttTime = 0; 
            parent.StartCoroutine(Attack()); 
        }

        if(parent.myTarget != null) 
        {
            
            float distance = Vector2.Distance(parent.myTarget.transform.parent.position, parent.transform.parent.position);
            if(distance >= parent.EnemyAttRange+extraRange && !parent.IsAttacking) 
            {
                if(parent is RangedEnemy)
                {
                    parent.ChangeState(new PathState());
                }
                else
                {
                    parent.ChangeState(new FollowState());
                }
            }
        }
        else 
        {
            parent.ChangeState(new IdleState());
        }
    }
    public IEnumerator Attack()
    {
        parent.IsAttacking = true;
        parent.MyAnim.SetTrigger("attack");
        yield return new WaitForSeconds(parent.MyAnim.GetCurrentAnimatorStateInfo(2).length);
        parent.IsAttacking = false;
    }
}
