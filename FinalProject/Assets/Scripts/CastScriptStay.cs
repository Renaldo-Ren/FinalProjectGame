using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastScriptStay : MonoBehaviour
{
    private Rigidbody2D Rb;
    [SerializeField]
    private float folSpeed;

    private Transform target;
    //[SerializeField]
    //private Player player;
    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position;
        //Vector2 dir = target.position - transform.position;
        //Rb.velocity = dir.normalized * folSpeed;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
