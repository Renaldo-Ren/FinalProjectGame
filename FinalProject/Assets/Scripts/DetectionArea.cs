using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionArea : MonoBehaviour
{
    private Enemy parent;
    //public List<Collider2D> detectionObj = new List<Collider2D>();
    //public Collider2D Areacollide;
    private void Start()
    {
        parent = GetComponentInParent<Enemy>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.gameObject.tag == "Player")
        //{
        //    detectionObj.Add(collision);
        //}
        if(collision.tag == "Player")
        {
            parent.Target = collision.transform;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Player")
        //{
        //    detectionObj.Remove(collision);
        //}
        if (collision.tag == "Player")
        {
            parent.Target = null;
        }
    }
}
