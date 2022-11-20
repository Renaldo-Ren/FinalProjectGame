using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    //[SerializeField]
    //private Stat health;

    [SerializeField]
    private Stat mana;
    
    
    private float initMP = 50;
    private Vector2 initPos;

    //[SerializeField]
    //private GameObject[] castPrefab;
    [SerializeField]
    private Enemy enemy;

    [SerializeField]
    private Guider guider;

    [SerializeField]
    private Block[] blocks;

    [SerializeField]
    private Transform[] exitPoints;

    private int exitIndex =2;

    private IInteractable interactable;
    private bool inRangeInteract = false;

    private SkillSet skillSet;

    //private Stack<Vector3> path;
    private Vector3 destination;
    private Vector3 goal;
    [SerializeField]
    private AStar astar;

    private Vector3 min, max;

    //public Transform myTarget { get; set; }
    public bool isCoolDown = false;
    private static Player instance;
    public static Player MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }
    private List<Enemy> attackers = new List<Enemy>();

    public List<Enemy> MyAttackers { get => attackers; set => attackers = value; }

    // Start is called before the first frame update
    protected override void Start()
    {
        skillSet = GetComponent<SkillSet>();
        
        mana.Initialize(initMP, initMP);
        initPos = transform.position;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!guider.Isinteracting)
        {
            GetInput();
        }
        ClickToMove();
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);
        base.Update();
    }
    public void SetDefaultValues()
    {
        initPos = transform.position;
    }

    private void GetInput()
    {
        Direction = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.I))
        {
            health.MyCurrentValue -= 10;
            mana.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MyCurrentValue += 10;
            mana.MyCurrentValue += 10;
        }

        if (Input.GetKey(KeyCode.W))
        {
            exitIndex = 0;
            Direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            exitIndex = 3;
            Direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            exitIndex = 2;
            Direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            exitIndex = 1;
            Direction += Vector2.right;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsAttacking && !isMoving && !UIManage.isPaused)
            {
                StartCoroutine(Attack());
            }
            //attackCoroutine = StartCoroutine(Attack());
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (inRangeInteract && !guider.Isinteracting)
            {
                Interact();
            }
        }
        if (isMoving)
        {
            StopAttack();
            StopCast();
        }
    }

    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    private IEnumerator Attack()
    {
        IsAttacking = true;
        MyAnim.SetBool("attack", IsAttacking); //Start attack animation
        yield return new WaitForSeconds(0.2f);
        StopAttack();
    }
    private IEnumerator Cast(int skillIndex)
    {
        Cast newSkill = skillSet.castSkill(skillIndex);
        isCasting = true;
        MyAnim.SetBool("cast", isCasting); //Start cast animation
        //yield return new WaitForSeconds(0.2f);
        yield return new WaitForSeconds(newSkill.myCastTime);
        if (skillIndex == 0)
        {
            if(myTarget != null && InLineofSight())
            {
                CastScript s = Instantiate(newSkill.myCastPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<CastScript>();
                s.Initialize(myTarget.MyHitbox, newSkill.myDamage, this);
            }
        }
        else
        {
            Instantiate(newSkill.myCastPrefab, transform.position, Quaternion.identity);
        }
        
        StopCast();
    }

    public void Casting(int skillIndex)
    {
        //Cast checkCD = skillSet.castSkill(skillIndex);
        Block();
        if (myTarget != null && myTarget.GetComponentInParent<Character>().IsAlive && !isCasting && InLineofSight() && !isMoving && IsAlive && !UIManage.isPaused)
        {
            attackCoroutine = StartCoroutine(Cast(skillIndex));
        }
        //attackCoroutine = StartCoroutine(Cast());
        
    }
    private bool InLineofSight()
    {
        if(myTarget != null)
        {
            Vector3 targetDir = (myTarget.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDir, Vector2.Distance(transform.position, myTarget.transform.position), 128);
            //If raycast didn't hit blocks then it's true in sight
            if (hit.collider == null)
            {
                return true;
            }
        }
        return false;
    }
    private void Block()
    {
        foreach(Block b in blocks)
        {
            b.Deactive();
        }
        blocks[exitIndex].Active();
    }

    public override void TakeDmg(float dmg, Character source, Vector2 knockback)
    {
            //Rigidbody2D PlayerRb = GetComponent<Rigidbody2D>();
            if (IsAlive)
            {
                base.TakeDmg(dmg, source, knockback);
                if (health.MyCurrentValue <= 0)
                {
                    knockback = Vector2.zero;
                }
            MyRb.AddForce(knockback);
            }
        //Debug.Log("Force" + knockback);
    }

    public void PlayerTakeForce(Vector2 knockback)
    {
        Rigidbody2D PlayerRb = GetComponent<Rigidbody2D>();
        //base.TakeDmg(dmg);
        //OnHealthChanged(health.MyCurrentValue);
        if (health.MyCurrentValue <= 0)
        {
            knockback = Vector2.zero;
        }
        PlayerRb.AddForce(knockback);
        //Debug.Log("Force" + knockback);
    }
    //public override void StopCast()
    //{
    //    skillSet.StopCasting();
    //    base.StopCast();
    //}
    public void StopAttack()
    {
        IsAttacking = false;
        MyAnim.SetBool("attack", IsAttacking);
        enemy.IsHit = false;
        //if (attackCoroutine != null)
        //{
        //    StopCoroutine(attackCoroutine);

        //}
    }
    public void StopCast()
    {
        //skillSet.StopCasting(); //Stop the skillset  from casting
        isCasting = false; //Makes sure that we are not attacking
        enemy.IsHit = false;
        MyAnim.SetBool("cast", isCasting); //Stops the cast animation
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }
    public void AddAttacker(Enemy enemy)
    {
        if (!MyAttackers.Contains(enemy))
        {
            MyAttackers.Add(enemy);
        }
    }
    public void Interact()
    {
        if(interactable != null)
        {
            interactable.Interact();
        }
    }

    public void GetPath(Vector3 goal)
    {
        MyPath = astar.Algorithm(transform.position, goal);
        destination = MyPath.Pop();
        this.goal = goal;
    }

    public IEnumerator Respawn()
    {
        MySpriteRenderer.enabled = false;
        yield return new WaitForSeconds(3f);
        health.Initialize(initHP, initHP);
        mana.Initialize(initMP, initMP);
        transform.position = initPos;
        MySpriteRenderer.enabled = true;
        MyAnim.SetTrigger("respawn");
    }

    private void ClickToMove()
    {
        if (MyPath != null)
        {
            transform.parent.position = Vector2.MoveTowards(transform.parent.position, destination, Speed * Time.deltaTime);
            float distance = Vector2.Distance(destination, transform.parent.position);
            if(distance <= 0f)
            {
                if(MyPath.Count > 0)
                {
                    destination = MyPath.Pop();
                }
                else
                {
                    MyPath = null;
                }
            }
        }
    }
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" || collision.tag =="Interactable")
        {
            interactable = collision.GetComponent<IInteractable>();
            inRangeInteract = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            if(interactable != null)
            {
                interactable.StopInteract();
                interactable = null;
                inRangeInteract = false;
            }
        }
    }

}