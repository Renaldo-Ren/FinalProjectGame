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
    [SerializeField]
    private Transform hitbox;
    [SerializeField]
    protected Stat health;
    [SerializeField]
    protected float initHP;

    private Vector2 direction;
    protected bool isCasting = false;
    protected Coroutine attackCoroutine;

    public bool IsAttacking { get; set; }
    public Character myTarget { get; set; }
    public Stack<Vector3> MyPath { get; set; }
    public Transform myCurrentTile { get; set; } 
    public List<Character> Attackers { get; set; } = new List<Character>();
    public Vector2 Direction { get => direction; set => direction = value; }
    public float Speed { get => speed; set => speed = value; }
    public Rigidbody2D MyRb { get => myRb; }
    public Animator MyAnim { get => myAnim; }
    public SpriteRenderer MySpriteRenderer { get => mySprite; }
    public Transform MyHitbox { get => hitbox; set => hitbox = value; }
    public Stat MyHealth
    {
        get { return health; }
    }
    
    public bool isMoving
    {
        get
        {
            return Direction.x != 0 || Direction.y != 0;
        }
    }

    public bool IsAlive
    {
        get
        {
            return health.MyCurrentValue > 0;
        }
    }

    protected virtual void Start()
    {
        health.Initialize(initHP, initHP);
    }

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

    public void HandleLayers()
    {
        if (IsAlive)
        {
            if (isMoving && IsAttacking)
            {
                ActivateLayers("Attack_Layer");
                MyAnim.SetFloat("x", Direction.x);
                MyAnim.SetFloat("y", Direction.y);
            }
            else if (isMoving && isCasting)
            {
                ActivateLayers("Cast_Layer");
                MyAnim.SetFloat("x", Direction.x);
                MyAnim.SetFloat("y", Direction.y);
            }
            else if (isMoving)
            {
                ActivateLayers("Walk_Layer");
                MyAnim.SetFloat("x", Direction.x);
                MyAnim.SetFloat("y", Direction.y);
            }
            else if (IsAttacking)
            {
                ActivateLayers("Attack_Layer");
            }
            else if (isCasting)
            {
                ActivateLayers("Cast_Layer");
            }
            else
            {
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
   
    public virtual void TakeDmg(float dmg, Character source, Vector2 knockback)
    {
        StartCoroutine(GetHit());
        
        if(Random.value < 0.3f)
        {
            dmg *= 2;
            CombatTextManage.MyInstance.CreateText(transform.position, dmg.ToString(), SCTTYPE.DAMAGE, true);
        }
        else
        {
            CombatTextManage.MyInstance.CreateText(transform.position, dmg.ToString(), SCTTYPE.DAMAGE, false);
        }
        health.MyCurrentValue -= dmg;

        if (health.MyCurrentValue <= 0)
        {
            Direction = Vector2.zero;
            MyRb.velocity = Direction;
            MyAnim.SetTrigger("die");
        }
    }

    public IEnumerator GetHit()
    {
        MySpriteRenderer.color = new Color(1f, 0.3056604f, 0.3056604f, 1f);
        yield return new WaitForSeconds(.3f);
        MySpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
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
