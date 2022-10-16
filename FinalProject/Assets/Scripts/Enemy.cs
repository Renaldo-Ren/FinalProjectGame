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

    
    public DetectionArea detectArea;

    private Transform target;
    private IState curState;

    public Transform Target { get => target; set => target = value; }

    protected void Awake()
    {
        ChangeState(new IdleState());
    }
    protected override void Update()
    {
        curState.Update();
        base.Update();
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
    public void TakeDmg(float dmg, Vector2 knockback) 
    {
        Rigidbody2D EnemyRb = GetComponent<Rigidbody2D>();
        base.TakeDmg(dmg);
        OnHealthChanged(health.MyCurrentValue);
        EnemyRb.AddForce(knockback);
        //Debug.Log("Force" + knockback);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //offset for collision detection changes the direction where the force comes from
        Vector2 dir = (Vector2)(collision.gameObject.transform.position - transform.position).normalized;
        
        //knockback is in direction of swordCollider towards collider
        Vector3 knockback = dir * thrust;

        if (collision.collider.tag == "Player")
        {
            collision.collider.GetComponentInParent<Player>().TakeDmg(3);
            collision.collider.GetComponentInParent<Player>().PlayerTakeForce(knockback);
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
        if(curState != null)
        {
            curState.Exit();
        }
        curState = newState;
        curState.Enter(this);
    }
}
