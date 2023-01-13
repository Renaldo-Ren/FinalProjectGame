using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private Transform[] exitPoints;

    protected override void Update()
    {
        LookAtTarget();
        base.Update();
    }

    public void Shoot(int exitIndex)
    {
        CastScript s = Instantiate(projectilePrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<CastScript>();
        s.Initialize(myTarget.MyHitbox, damage, this);
    }
    private void LookAtTarget()
    {
        if(myTarget != null)
        {
            Vector2 directionToTarget = (myTarget.transform.position - transform.position).normalized;
            Vector2 facing = new Vector2(MyAnim.GetFloat("x"), 0f);
            if (GetComponentInParent<Enemy>().IsAlive) 
            {
                if (directionToTarget.x >= 0)
                {
                    directionToTarget.x = 1;
                }
                else
                {
                    directionToTarget.x = -1;
                }
                MyAnim.SetFloat("x", directionToTarget.x);
            }
        }
    }
}
