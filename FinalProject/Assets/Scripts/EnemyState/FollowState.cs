using UnityEngine;

class FollowState : IState
{
    private Enemy parent;
    //private Vector3 offset;
    public void Enter(Enemy parent)
    {
        Player.MyInstance.AddAttacker(parent);
        this.parent = parent;
        parent.MyPath = null;
    }

    public void Exit() // called everytime exit a state
    {
        //Set the direction back to zero, so that the enemy could stop move after follow
        parent.Direction = Vector2.zero;
        parent.MyRb.velocity = Vector2.zero;
    }

    public void Update()
    {
        if(parent.myTarget != null)
        {
            //Find the target's direction
            parent.Direction = ((parent.myTarget.transform.position) - parent.transform.position).normalized;

            //Enemy moves towards the target
            //parent.transform.position = Vector2.MoveTowards(parent.transform.position, parent.myTarget.position, parent.Speed * Time.deltaTime);

            float distance = Vector2.Distance(parent.myTarget.position, parent.transform.position);
            if(distance <= parent.EnemyAttRange) //target enter the enemy attack range
            {
                parent.ChangeState(new AttackState());
                
            }
        }
        if (!parent.inRange) //if Player is not in enemy range, then back to evade state/back to start position
        {
            parent.ChangeState(new EvadeState());
        }
        else if (!parent.CanSeePlayer())
        {
            parent.ChangeState(new PathState());
        }

    }
}
