using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField]
    private Stat health;

    [SerializeField]
    private Stat mana;

    private float initHP = 100;
    private float initMP = 50;

    [SerializeField]
    private GameObject[] castPrefab;

    [SerializeField]
    private Block[] blocks;

    [SerializeField]
    private Transform[] exitPoints;

    private int exitIndex =2;

    public Transform myTarget { get; set; }

    // Start is called before the first frame update
    protected override void Start()
    {
        health.Initialize(initHP, initHP);
        mana.Initialize(initMP, initMP);

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();

        base.Update();
    }
    
    private void GetInput()
    {
        direction = Vector2.zero;
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
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            exitIndex = 3;
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            exitIndex = 2;
            direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            exitIndex = 1;
            direction += Vector2.right;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (!isAttacking)
            {
                StartCoroutine(Attack());
            }
            //attackCoroutine = StartCoroutine(Attack());
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            
        }
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
        isCasting = true;
        anim.SetBool("cast", isCasting); //Start cast animation
        yield return new WaitForSeconds(0.2f);
        
        Instantiate(castPrefab[skillIndex], exitPoints[exitIndex].position, Quaternion.identity);
        StopCast();
    }

    public void Casting(int skillIndex)
    {
        Block();
        if (myTarget != null && !isCasting && InLineofSight())
        {
            StartCoroutine(Cast(skillIndex));
        }
        //attackCoroutine = StartCoroutine(Cast());
        
    }
    private bool InLineofSight()
    {
        Vector3 targetDir = (myTarget.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDir, Vector2.Distance(transform.position, myTarget.transform.position), 128);
        //If raycast didn't hit blocks then it's true in sight
        if(hit.collider == null)
        {
            return true;
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
}
