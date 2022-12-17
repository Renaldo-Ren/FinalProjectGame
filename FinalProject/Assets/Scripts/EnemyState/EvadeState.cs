using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : IState
{
    private Enemy parent;
    public void Enter(Enemy parent)
    {
        this.parent = parent;
        if(parent.myTarget != null)
        {
            parent.myTarget.RemoveAttacker(parent);
        }
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
        parent.ResetEnemy();
    }

    public void Update()
    {
        parent.Direction = (parent.myStartPos - parent.transform.position).normalized;
        //parent.transform.position = Vector2.MoveTowards(parent.transform.position, parent.myStartPos, parent.Speed*Time.deltaTime);
        float distance = Vector2.Distance(parent.myStartPos, parent.transform.parent.position);
        parent.StartCoroutine(RevertBack());
        if (distance <= 0.1f)
        {
            parent.ChangeState(new IdleState());
            parent.StopAllCoroutines();
        }
        
    }

    private IEnumerator RevertBack()
    {
        yield return new WaitForSeconds(5f);
        parent.transform.parent.position = parent.myStartPos;
    }
}
