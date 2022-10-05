using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    protected Animator anim;
    [SerializeField]
    private Rigidbody2D rb;

    protected Vector2 direction;
    protected bool isAttacking = false;
    protected bool isCasting = false;
    protected Coroutine attackCoroutine;
    public bool isMoving
    {
        get
        {
            return direction.x != 0 || direction.y != 0;
        }
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleLayers();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        //Frame Rate Independent
        rb.velocity = direction.normalized * speed;

        //Frame Rate Dependent
        //transform.Translate(direction * speed * Time.deltaTime);
    }

    public void HandleLayers()
    {
        //If player is attacking while moving, then animate player attack and move
        if (isMoving && isAttacking)
        {
            ActivateLayers("Attack_Layer");

            //Sets the animation parameter so that he faces the correct direction
            anim.SetFloat("x", direction.x);
            anim.SetFloat("y", direction.y);
        }
        //If player is attacking while moving, then animate player attack and move
        else if (isMoving && isCasting)
        {
            ActivateLayers("Cast_Layer");

            //Sets the animation parameter so that he faces the correct direction
            anim.SetFloat("x", direction.x);
            anim.SetFloat("y", direction.y);
        }
        //If player is moving, then animate player movement
        else if (isMoving)
        {
            ActivateLayers("Walk_Layer");

            //Sets the animation parameter so that he faces the correct direction
            anim.SetFloat("x", direction.x);
            anim.SetFloat("y", direction.y);
        }
        //If player is attacking, then animate player attack
        else if (isAttacking)
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

    public void ActivateLayers(string layername)
    {
        for (int i=0; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }
        anim.SetLayerWeight(anim.GetLayerIndex(layername), 1);
    }

    public void StopAttack()
    {
        isAttacking = false;
        anim.SetBool("attack", isAttacking);
        //if(attackCoroutine != null)
        //{
        //StopCoroutine(attackCoroutine);

        //}
    }
    public virtual void StopCast()
    {
        isCasting = false;
        anim.SetBool("cast", isCasting);
        //if (attackCoroutine != null)
        //{
        //    StopCoroutine(attackCoroutine);
        //}
    }
}
