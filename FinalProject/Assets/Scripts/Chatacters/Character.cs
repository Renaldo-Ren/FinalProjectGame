using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Animator myAnim;
    [SerializeField]
    private Rigidbody2D myRb;
    [SerializeField]
    private SpriteRenderer mySprite;

    private Vector2 direction;
    
    public bool IsAttacking { get; set; }
    protected bool isCasting = false;
    public bool IsHit { get; set; }
    //protected bool isHit = false;
    protected Coroutine attackCoroutine;

    [SerializeField]
    private Transform hitbox;

    [SerializeField]
    protected Stat health;

    //public Transform myTarget { get; set; }
    public Character myTarget { get; set; }
    public Stack<Vector3> MyPath { get; set; }

    public Transform myCurrentTile { get; set; } //this is a tile that the player currently standing

    public List<Character> Attackers { get; set; } = new List<Character>();
    public Stat MyHealth
    {
        get { return health; }
    }

    [SerializeField]
    protected float initHP;
    public bool isMoving
    {
        get
        {
            return Direction.x != 0 || Direction.y != 0;
        }
    }

    public Vector2 Direction { get => direction; set => direction = value; }
    public float Speed { get => speed; set => speed = value; }
    public bool IsAlive
    {
        get
        {
            return health.MyCurrentValue > 0;
        }
    }

    public Rigidbody2D MyRb { get => myRb; }
    public Animator MyAnim { get => myAnim; }
    public SpriteRenderer MySpriteRenderer { get => mySprite; }
    public Transform MyHitbox { get => hitbox; set => hitbox = value; }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //MyAnim = GetComponent<Animator>();
        //MyRb = GetComponent<Rigidbody2D>();
        health.Initialize(initHP, initHP);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(MyAnim != null)
        {
            HandleLayers();
        }
    }
    public void FixedUpdate()
    {
        Move();
    }
    public void Move()
    {
        if (MyPath == null)
        {
            if (IsAlive)
            {
                MyRb.velocity = Direction.normalized * Speed;
            }
        }
    }
    //private void FixedUpdate()
    //{
    //    Move();
    //}

    //public void Move()
    //{
    //    if (IsAlive)
    //    {
    //        //Frame Rate Independent
    //        MyRb.velocity = Direction.normalized * Speed;

    //        //Frame Rate Dependent
    //        //transform.Translate(direction * speed * Time.deltaTime);
    //    }

    //}

    public void HandleLayers()
    {
        if (IsAlive)
        {
            //If player is attacking while moving, then animate player attack and move
            if (isMoving && IsAttacking)
            {
                ActivateLayers("Attack_Layer");

                //Sets the animation parameter so that he faces the correct direction
                MyAnim.SetFloat("x", Direction.x);
                MyAnim.SetFloat("y", Direction.y);
            }
            //If player is attacking while moving, then animate player attack and move
            else if (isMoving && isCasting)
            {
                ActivateLayers("Cast_Layer");

                //Sets the animation parameter so that he faces the correct direction
                MyAnim.SetFloat("x", Direction.x);
                MyAnim.SetFloat("y", Direction.y);
            }
            //else if (IsHit)
            //{
            //    ActivateLayers("Hit_Layer");
            //}
            //If player is moving, then animate player movement
            else if (isMoving)
            {
                ActivateLayers("Walk_Layer");

                //Sets the animation parameter so that he faces the correct direction
                MyAnim.SetFloat("x", Direction.x);
                MyAnim.SetFloat("y", Direction.y);
            }
            //If player is attacking, then animate player attack
            else if (IsAttacking)
            {
                ActivateLayers("Attack_Layer");
            }
            //If player is attacking, then animate player attack
            else if (isCasting)
            {
                ActivateLayers("Cast_Layer");
            }
            
            else
            {
                //Else, back to idle animation
                ActivateLayers("Idle_Layer");
            }
        }
        else
        {
            ActivateLayers("Death_Layer");
        }
    }

    public void ActivateLayers(string layername)
    {
        for (int i=0; i < MyAnim.layerCount; i++)
        {
            MyAnim.SetLayerWeight(i, 0);
        }
        MyAnim.SetLayerWeight(MyAnim.GetLayerIndex(layername), 1);
    }

    //public void StopAttack()
    //{
    //    isAttacking = false;
    //    anim.SetBool("attack", isAttacking);
    //    //if (attackCoroutine != null)
    //    //{
    //    //    StopCoroutine(attackCoroutine);

    //    //}
    //}
   
    public virtual void TakeDmg(float dmg, Character source, Vector2 knockback)
    {
        //if(myTarget == null)
        //{
        //    myTarget = source;
        //}
        IsHit = true;
        MyAnim.SetTrigger("hit");
        StartCoroutine(GetHit());
        health.MyCurrentValue -= dmg;
        CombatTextManage.MyInstance.CreateText(transform.position, dmg.ToString(), SCTTYPE.DAMAGE, false);
        if (health.MyCurrentValue <= 0)
        {
            Direction = Vector2.zero;
            MyRb.velocity = Direction;
            //GameManage.MyInstance.OnKillConfirmed(this);
            MyAnim.SetTrigger("die");
        }
        //isHit = false;
    }

    public void GetHealth(int health)
    {
        MyHealth.MyCurrentValue += health;
        CombatTextManage.MyInstance.CreateText(transform.position, health.ToString(), SCTTYPE.HEAL, false);
    }

    public IEnumerator GetHit()
    {
        MySpriteRenderer.color = new Color(1f, 0.3056604f, 0.3056604f, 1f);
        yield return new WaitForSeconds(.3f);
        MySpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        //Debug.Log("character: "+ mySprite +", color: "+ MySpriteRenderer.color);
    }
    public virtual void AddAttacker(Character attacker)
    {
        if (!Attackers.Contains(attacker))
        {
            Attackers.Add(attacker);
        }
    }

    public virtual void RemoveAttacker(Character attacker)
    {
        Attackers.Remove(attacker);
    }
}
