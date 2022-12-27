using UnityEngine;

class FollowState : IState
{
    private Enemy parent;
    public void Enter(Enemy parent)
    {
        this.parent = parent;
        parent.MyPath = null;
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
        parent.MyRb.velocity = Vector2.zero;
    }

    public void Update()
    {
        if(parent.myTarget != null)
        {
            parent.Direction = ((parent.myTarget.transform.position) - parent.transform.position).normalized;

            float distance = Vector2.Distance(parent.myTarget.transform.position, parent.transform.position);
            if(distance <= parent.EnemyAttRange) 
            {
                parent.ChangeState(new AttackState());
                
            }
        }
        if (!parent.inRange) 
        {
            parent.ChangeState(new EvadeState());
        }
        else if (!parent.CanSeePlayer())
        {
            parent.ChangeState(new PathState());
        }

    }
}
