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
        this.parent.ResetEnemy();
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        if (parent.myTarget != null)
        {
            parent.ChangeState(new PathState());
        }
    }
}
