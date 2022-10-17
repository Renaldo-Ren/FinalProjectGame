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

    //[SerializeField]
    //private GameObject[] castPrefab;

    [SerializeField]
    private Block[] blocks;

    [SerializeField]
    private Transform[] exitPoints;

    private int exitIndex =2;

    private SkillSet skillSet;

    private Vector3 min, max;

    public Transform myTarget { get; set; }
    public bool isCoolDown = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        skillSet = GetComponent<SkillSet>();
        
        mana.Initialize(initMP, initMP);

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);
        base.Update();
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
            if (!isAttacking && !isMoving)
            {
                StartCoroutine(Attack());
            }
            //attackCoroutine = StartCoroutine(Attack());
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
        isAttacking = true;
        anim.SetBool("attack", isAttacking); //Start attack animation
        yield return new WaitForSeconds(0.2f);
        StopAttack();
    }
    private IEnumerator Cast(int skillIndex)
    {
        Cast newSkill = skillSet.castSkill(skillIndex);
        isCasting = true;
        anim.SetBool("cast", isCasting); //Start cast animation
        //yield return new WaitForSeconds(0.2f);
        yield return new WaitForSeconds(newSkill.myCastTime);
        if (skillIndex == 0)
        {
            if(myTarget != null && InLineofSight())
            {
                CastScript s = Instantiate(newSkill.myCastPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<CastScript>();
                s.Initialize(myTarget, newSkill.myDamage);
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
        if (myTarget != null && !isCasting && InLineofSight())
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
    public void PlayerTakeForce(Vector2 knockback)
    {
        Rigidbody2D PlayerRb = GetComponent<Rigidbody2D>();
        //base.TakeDmg(dmg);
        //OnHealthChanged(health.MyCurrentValue);
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
        isAttacking = false;
        anim.SetBool("attack", isAttacking);
        //if (attackCoroutine != null)
        //{
        //    StopCoroutine(attackCoroutine);

        //}
    }
    public void StopCast()
    {
        //skillSet.StopCasting(); //Stop the skillset  from casting
        isCasting = false; //Makes sure that we are not attacking
        anim.SetBool("cast", isCasting); //Stops the cast animation
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }
}
