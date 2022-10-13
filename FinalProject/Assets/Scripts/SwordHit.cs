using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour
{
    [SerializeField]
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 dir = Vector2.zero;
        transform.position = player.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "HitBox")
        {
            collision.GetComponentInParent<Enemy>().TakeDmg(3);
            collision.GetComponentInParent<Enemy>().HPgroup.alpha = 1;
        }
    }
}
