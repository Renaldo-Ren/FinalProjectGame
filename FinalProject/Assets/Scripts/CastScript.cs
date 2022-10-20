using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastScript : MonoBehaviour
{
    private Rigidbody2D Rb;
    [SerializeField]
    private float speed;

    private Transform target;
    public Transform myTarget { get; private set;  }
    private Transform source;

    private int damage;
    public float thrust = 50f;
    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
    }
    public void Initialize(Transform target, int damage, Transform source)
    {
        this.myTarget = target;
        this.damage = damage;
        this.source = source;
    }
    // Update is called once per frame
    void Update()
    {
        //Vector2 dir = target.position - transform.position;
        //Rb.velocity = dir.normalized * speed;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    private void FixedUpdate()
    {
        if (myTarget != null)
        {
            Vector2 dir = myTarget.position - transform.position;
            Rb.velocity = dir.normalized * speed;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 parentPos = gameObject.GetComponentInParent<Transform>().position;
        Vector2 dir = (Vector2)(collision.gameObject.transform.position - parentPos).normalized;
        Vector3 knockback = dir * thrust;
        if (collision.tag == "HitBox" && collision.transform == myTarget)
        {
            //Character c = collision.GetComponentInParent<Character>();
            speed = 0;
            //c.TakeDmg(damage, source);
            collision.GetComponentInParent<Enemy>().TakeDmg(damage, source, knockback);
            GetComponent<Animator>().SetTrigger("impact");
            Rb.velocity = Vector2.zero;
            myTarget = null;
        }
    }
}
