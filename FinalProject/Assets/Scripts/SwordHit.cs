using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour
{
    [SerializeField]
    private Player player;
    public float thrust = 1200f;
    private Character source;
    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        this.source = GetComponentInParent<Character>();

    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 dir = Vector2.zero;
        transform.position = player.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //calculate direction between character and target
        Vector3 parentPos = transform.parent.position;
        //Offset for collision detection changes the direction where the force comes from (close to the player)
        Vector2 dir = (Vector2)(collision.gameObject.transform.position - parentPos).normalized;
        //knockback is in direction of swordCollider towards collider
        Vector3 knockback = dir * thrust;

        if (collision.tag == "HitBox")
        {
            Character c = collision.GetComponentInParent<Character>();
            c.TakeDmg(3, source, knockback);
            collision.GetComponentInParent<Enemy>().HPgroup.alpha = 1;
            //collision.GetComponentInParent<Enemy>().IsHit = false;
            //collision.GetComponentInParent<Enemy>().TakeForce(knockback);
        }
    }
}
