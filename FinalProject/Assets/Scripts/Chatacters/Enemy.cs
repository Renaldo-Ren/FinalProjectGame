using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    [SerializeField]
    public CanvasGroup HPgroup;
    
    [SerializeField]
    private AStar astar;

    [SerializeField]
    protected int damage;

    [SerializeField]
    private LayerMask losMask;

    [SerializeField]
    private float attackRange;

    [SerializeField]
    private float initAggroRange;

    public DetectionArea detectArea;
    private IState curState;
    private bool canDoDamage = true;
    public float thrust = 2000f;
    public float EnemyAttTime { get; set; }
    public Vector3 myStartPos { get; set; }
    public float EneAggroRange { get; set; }
    public AStar EneAstar { get => astar; }
    public float EnemyAttRange { get => attackRange; set => attackRange = value; }
    public bool inRange
    {
        get
        {
            return Vector2.Distance(transform.position, myTarget.transform.position) < EneAggroRange;
        }
    }

    protected void Awake()
    {
        myStartPos = transform.position; //so the enemy will know where it reset to
        EneAggroRange = initAggroRange;
        ChangeState(new IdleState());
    }

    protected override void Update()
    {
        if (IsAlive)
        {
            if (!IsAttacking)
            {
                EnemyAttTime += Time.deltaTime;
            }

            curState.Update();

            if(myTarget != null && !Player.MyInstance.IsAlive)
            {
                ChangeState(new EvadeState());
            }
        }
        base.Update(); //Make sure in outside if alive so the enemy can use handlelayer function to make it death layer when die
    }

    public override Character Select()
    {
        HPgroup.alpha = 1;
        return base.Select();
    }
    public override void DeSelect()
    {
        HPgroup.alpha = 0;
        base.DeSelect();
    }
    public override void TakeDmg(float dmg, Character source, Vector2 knockback) 
    {
        if(!(curState is EvadeState)) //if the enemy current state is not in evade state, then it will take damage
        {
            if (IsAlive)
            {
                setTarget(source); //when take damage, set the target based on the source who damage it
                base.TakeDmg(dmg, source, knockback);
                OnHealthChanged(health.MyCurrentValue);
                if (health.MyCurrentValue <= 0)
                {
                    source.RemoveAttacker(this); //Remove this enemy as player attacker
                    knockback = Vector2.zero;
                }
                MyRb.AddForce(knockback);
            }
            
        }
    }

    public void DoDamage()
    {
        if (canDoDamage)
        {
            //offset for collision detection changes the direction where the force comes from
            Vector2 dir = (Vector2)(myTarget.transform.position - transform.position).normalized;

            //knockback is in direction of swordCollider towards collider
            Vector3 knockback = dir * thrust;

            myTarget.TakeDmg(damage, this, knockback);
            canDoDamage = false;
        }
    }
    public void AbleToDamage()
    {
        canDoDamage = true; //used to reset back to can do dmg
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            //offset for collision detection changes the direction where the force comes from
            Vector2 dir = (Vector2)(collision.gameObject.transform.position - transform.position).normalized;

            //knockback is in direction of swordCollider towards collider
            Vector3 knockback = dir * thrust;
            collision.collider.GetComponentInParent<Player>().TakeDmg(3, this, knockback); //player get 3 dmg from the enemy itself when collide
        }
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{

    //    Vector3 parentPos = gameObject.GetComponentInParent<Transform>().position;
    //    Vector2 dir = (Vector2)(collision.gameObject.transform.position - transform.position).normalized;
    //    Vector3 knockback = dir * thrust;

    //    if (collision.tag == "Player")
    //    {
    //        collision.GetComponentInParent<Player>().TakeDmg(3);
    //        //collision.GetComponentInParent<Player>().HPgroup.alpha = 1;
    //        collision.GetComponentInParent<Player>().PlayerTakeForce(knockback);
    //    }
    //}

    public void ChangeState(IState newState)
    {
        if(curState != null) //Make sure have a state before call exit
        {
            curState.Exit();
        }
        curState = newState; //Sets the new state
        curState.Enter(this); //Call enter on the new state
    }
    public void setTarget(Character target)
    {
        if(myTarget == null && !(curState is EvadeState)) //if no have target and not in evade state, then set the target
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            EneAggroRange = initAggroRange; //this to make sure we set it to start/reset it so when it added by the distance later, it will not get too far
            EneAggroRange += distance;
            myTarget = target;
            Debug.Log(myTarget);
            target.AddAttacker(this);
        }
    }

    public void ResetEnemy()
    {
        this.myTarget = null;
        this.EneAggroRange = initAggroRange;
        this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxVal;
        OnHealthChanged(health.MyCurrentValue); //need also to reset the health frame so it will always keep track/same with the actual health value
    }
    public bool CanSeePlayer()
    {
        Vector3 targetDirection = (myTarget.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, myTarget.transform.position), losMask);
        if(hit.collider != null) //if the raycast not null
        {
            return false; //then cannot see player
        }
        return true;
    }
}
