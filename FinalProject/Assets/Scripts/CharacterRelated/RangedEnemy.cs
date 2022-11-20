using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private Transform[] exitPoints;

    private float fieldOfView = 120;
    private bool updateDirection = false;
    protected override void Update()
    {
        LookAtTarget();
        base.Update();
    }
    //private void LateUpdate()
    //{
    //    UpdateDirection();
    //}

    public void Shoot(int exitIndex)
    {
        CastScript s = Instantiate(projectilePrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<CastScript>();
        s.Initialize(myTarget.MyHitbox, damage, this);
    }
    //private void UpdateDirection()
    //{
    //    if (updateDirection)
    //    {
    //        Vector2 dir = Vector2.zero;
    //        if (MySpriteRenderer.sprite.name.Contains("(PlantLeft)"))
    //        {
    //            dir = Vector2.left;
    //        }
    //        else if (MySpriteRenderer.sprite.name.Contains("(PlantRight)"))
    //        {
    //            dir = Vector2.right;
    //        }
    //        MyAnim.SetFloat("x", dir.x);
    //        //MyAnim.SetFloat("y", dir.y);
    //        updateDirection = false;
    //    }
    //}
    private void LookAtTarget()
    {
        if(myTarget != null)
        {
            Vector2 directionToTarget = (myTarget.transform.position - transform.position).normalized;
            Vector2 facing = new Vector2(MyAnim.GetFloat("x"), 0f);
            if(directionToTarget.x >= 0)
            {
                directionToTarget.x = 1;
            }
            else
            {
                directionToTarget.x = -1;
            }
            float angleToTarget = Vector2.Angle(facing, directionToTarget);
            if (angleToTarget > fieldOfView/2)
            {
                MyAnim.SetFloat("x", directionToTarget.x);
                //MyAnim.SetFloat("y", directionToTarget.y);

                updateDirection = true;
            }
        }
    }
}
