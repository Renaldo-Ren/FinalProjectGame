using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    [SerializeField]
    public CanvasGroup HPgroup;
    
    //private Rigidbody2D EnemyRb;
    public float thrust = 2000f;
    public Collider2D collide;
    private float moveSpeed = 10f;

    [SerializeField]
    private AStar astar;

    [SerializeField]
    private int damage;

    private bool canDoDamage = true;

    [SerializeField]
    private LayerMask losMask;

    public DetectionArea detectArea;

    //private Transform target;
    private IState curState;

    public float EnemyAttRange { get; set; }
    public float EnemyAttTime { get; set; }

    public Vector3 myStartPos { get; set; }

    [SerializeField]
    private float initAggroRange;
    public float EneAggroRange { get; set; }

    public bool inRange
    {
        get
        {
            return Vector2.Distance(transform.position, myTarget.position) < EneAggroRange;
        }
    }

    public AStar EneAstar { get => astar; }

    //public Transform Target { get => target; set => target = value; }

    protected void Awake()
    {
        myStartPos = transform.position; //so the enemy will know where it reset to
        EneAggroRange = initAggroRange;
        EnemyAttRange = 1;
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

    //private void FixedUpdate()
    //{
    //    Rigidbody2D EnemyRb = GetComponent<Rigidbody2D>();
    //    if (detectArea.detectionObj.Count > 0)
    //    {
    //        Vector2 dir = (detectArea.detectionObj[0].transform.position - transform.position).normalized;

    //        EnemyRb.AddForce(dir * moveSpeed * Time.deltaTime);

    //        if(EnemyRb.velocity.magnitude > 5f)
    //        {
    //            float limitSpeed = Mathf.Lerp(EnemyRb.velocity.magnitude, 5f, 0.9f);
    //            EnemyRb.velocity = EnemyRb.velocity.normalized * limitSpeed;
    //        }
    //    }
    //}
    public override Transform Select()
    {
        HPgroup.alpha = 1;
        return base.Select();
    }
    public override void DeSelect()
    {
        HPgroup.alpha = 0;
        base.DeSelect();
    }
    //public override void TakeDmg(float dmg)
    //{
    //    base.TakeDmg(dmg);
    //    OnHealthChanged(health.MyCurrentValue);
    //}
    public void TakeDmg(float dmg, Transform source, Vector2 knockback) 
    {
        if(!(curState is EvadeState)) //if the enemy current state is not in evade state, then it will not take damage
        {
            Rigidbody2D EnemyRb = GetComponent<Rigidbody2D>();
            if (IsAlive)
            {
                setTarget(source); //when take damage, set the target based on the source who damage it
                base.TakeDmg(dmg, source);
                OnHealthChanged(health.MyCurrentValue);
                if (health.MyCurrentValue <= 0)
                {
                    Player.MyInstance.MyAttackers.Remove(this); //Remove this enemy as player attacker
                    knockback = Vector2.zero;
                }
                EnemyRb.AddForce(knockback);
            }
            
        }
        
        //Debug.Log("Force" + knockback);
    }

    public void DoDamage()
    {
        if (canDoDamage)
        {
            Player.MyInstance.TakeDmg(damage, transform);
            canDoDamage = false;
        }
    }
    public void AbleToDamage()
    {
        canDoDamage = true; //used to reset back to can do dmg
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //offset for collision detection changes the direction where the force comes from
        Vector2 dir = (Vector2)(collision.gameObject.transform.position - transform.position).normalized;
        
        //knockback is in direction of swordCollider towards collider
        Vector3 knockback = dir * thrust*0;

        if (collision.collider.tag == "Player")
        {
            collision.collider.GetComponentInParent<Player>().TakeDmg(3, transform); //player get 3 dmg from the enemy itself when collide
            collision.collider.GetComponentInParent<Player>().PlayerTakeForce(knockback);
            collision.collider.GetComponentInParent<Player>().IsHit = false;
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
    public void setTarget(Transform target)
    {
        if(myTarget == null && !(curState is EvadeState)) //if no have target and not in evade state, then set the target
        {
            float distance = Vector2.Distance(transform.position, target.position);
            EneAggroRange = initAggroRange; //this to make sure we set it to start/reset it so when it added by the distance later, it will not get too far
            EneAggroRange += distance;
            myTarget = target;
        }
    }

    public void ResetEnemy()
    {
        this.myTarget = null;
        this.EneAggroRange = initAggroRange;
        this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxVal;
        OnHealthChanged(health.MyCurrentValue); //need also to reset the health frame so it will always keep track/same with the actual health value
    }
    public override void Interact()
    {
       // base.Interact();
    }
    public override void StopInteract()
    {
        //base.StopInteract();

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
