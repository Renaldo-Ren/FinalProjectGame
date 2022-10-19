using UnityEngine;

class FollowState : IState
{
    private Enemy parent;
    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit() // called everytime exit a state
    {
        //Set the direction back to zero, so that the enemy could stop move after follow
        parent.Direction = Vector2.zero;
    }

    public void Update()
    {
        if(parent.Target != null)
        {
            //Find the target's direction
            parent.Direction = (parent.Target.transform.position - parent.transform.position).normalized;

            //Enemy moves towards the target
            parent.transform.position = Vector2.MoveTowards(parent.transform.position, parent.Target.position, parent.Speed * Time.deltaTime);

            float distance = Vector2.Distance(parent.Target.position, parent.transform.position);
            if(distance <= parent.EnemyAttRange) //target enter the enemy attack range
            {
                parent.ChangeState(new AttackState());
                
            }
        }
        else
        {
            parent.ChangeState(new IdleState());
        }

    }
}
