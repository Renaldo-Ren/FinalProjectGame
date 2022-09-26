using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    
    
    // Start is called before the first frame update
    void Start()
    {
        
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //attackCoroutine = StartCoroutine(Attack());
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            anim.SetBool("attack", isAttacking);
            yield return new WaitForSeconds(0.2f);
            StopAttack();
        }
        
    }
}
