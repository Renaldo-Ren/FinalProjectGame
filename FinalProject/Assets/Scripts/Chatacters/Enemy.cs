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
        myStartPos = transform.parent.position;
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

            if (myTarget != null && !Player.MyInstance.IsAlive)
            {
                ChangeState(new EvadeState());
            }
        }
        Debug.Log(myTarget);
        base.Update();
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
        if(!(curState is EvadeState)) 
        {
            if (IsAlive)
            {
                setTarget(source); 
                base.TakeDmg(dmg, source, knockback);
                OnHealthChanged(health.MyCurrentValue);
                if (health.MyCurrentValue <= 0)
                {
                    source.RemoveAttacker(this); 
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
            Vector2 dir = (Vector2)(myTarget.transform.position - transform.position).normalized;
            Vector3 knockback = dir * thrust;
            myTarget.TakeDmg(damage, this, knockback);
            canDoDamage = false;
        }
    }
    public void AbleToDamage()
    {
        canDoDamage = true; 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Vector2 dir = (Vector2)(collision.gameObject.transform.position - transform.position).normalized;
            Vector3 knockback = dir * thrust;
            collision.collider.GetComponentInParent<Player>().TakeDmg(3, this, knockback);
        }
    }

    public void ChangeState(IState newState)
    {
        if(curState != null)
        {
            curState.Exit();
        }
        curState = newState;
        curState.Enter(this);
    }
    public void setTarget(Character target)
    {
        if(myTarget == null && !(curState is EvadeState)) //if no have target and not in evade state, then set the target
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            EneAggroRange = initAggroRange; //this to make sure we set it to start/reset it so when it added by the distance later, it will not get too far
            EneAggroRange += distance;
            myTarget = target;
            target.AddAttacker(this);
        }
    }

    public void ResetEnemy()
    {
        this.myTarget = null;
        this.EneAggroRange = initAggroRange;
        this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxVal;
        OnHealthChanged(health.MyCurrentValue);
    }
    public bool CanSeePlayer()
    {
        Vector3 targetDirection = (myTarget.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, myTarget.transform.position), losMask);
        if(hit.collider != null)
        {
            return false;
        }
        return true;
    }
}
