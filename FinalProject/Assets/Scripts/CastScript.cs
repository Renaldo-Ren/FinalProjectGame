using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastScript : MonoBehaviour
{
    private Rigidbody2D Rb;
    [SerializeField]
    private float speed;

    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Target").transform; //just for debugging
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector2 dir = target.position - transform.position;
        Rb.velocity = dir.normalized * speed;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
