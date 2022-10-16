using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class IdleState : IState
{
    private Enemy parent;
    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        //Chane into follow state if the player is close
        if (parent.Target != null) //If have target, then follow it
        {
            parent.ChangeState(new FollowState());
        }
    }
}
