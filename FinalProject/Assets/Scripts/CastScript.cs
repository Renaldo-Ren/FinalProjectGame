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

    private int damage;
    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
    }
    public void Initialize(Transform target, int damage)
    {
        this.myTarget = target;
        this.damage = damage;
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
        if (collision.tag == "HitBox" && collision.transform == myTarget)
        {
            speed = 0;
            collision.GetComponentInParent<Enemy>().TakeDmg(damage);
            GetComponent<Animator>().SetTrigger("impact");
            Rb.velocity = Vector2.zero;
            myTarget = null;
        }
    }
}
