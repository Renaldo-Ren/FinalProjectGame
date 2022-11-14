using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private Transform[] exitPoints;

    public void Shoot(int exitIndex)
    {
        CastScript s = Instantiate(projectilePrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<CastScript>();
        s.Initialize(myTarget, damage, transform);
    }

}
