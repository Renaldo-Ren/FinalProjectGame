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
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
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
            if (!isCasting)
            {
                StartCoroutine(Cast());
            }
            //attackCoroutine = StartCoroutine(Cast());
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        anim.SetBool("attack", isAttacking); //Start attack animation
        yield return new WaitForSeconds(0.2f);
        StopAttack();
    }
    private IEnumerator Cast()
    {
        isCasting = true;
        anim.SetBool("cast", isCasting); //Start cast animation
        yield return new WaitForSeconds(0.3f);
        Casting();
        StopCast();
    }

    public void Casting()
    {
        Instantiate(castPrefab[0], transform.position, Quaternion.identity);
    }
}
