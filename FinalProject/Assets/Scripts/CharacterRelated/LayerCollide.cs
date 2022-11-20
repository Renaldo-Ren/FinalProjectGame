using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerCollide : MonoBehaviour
{
    private SpriteRenderer parentRenderer;

    private List<Obstacle> obstacles = new List<Obstacle>();
    // Start is called before the first frame update
    void Start()
    {
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If player hit obstacle
        if(collision.tag == "Obstacle")
        {
            //create a reference to the obstacle
            Obstacle o = collision.GetComponent<Obstacle>();
            o.FadeOut();
            //If we aren't colliding with anything else or we are colliding with some obstacles
            if(obstacles.Count == 0 || o.MySpriteRenderer.sortingOrder -1 < parentRenderer.sortingOrder)
            {
                //Change the sort order to behind what we just hit
                parentRenderer.sortingOrder = o.MySpriteRenderer.sortingOrder - 1;
            }
            obstacles.Add(o); //Adds obstacle to the list, so we can keep track it
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //if player stop collide with the obstacle
        if (collision.tag == "Obstacle")
        {
            //create a reference to the obstacle
            Obstacle o = collision.GetComponent<Obstacle>();
            o.Fadein();
            obstacles.Remove(o); //Remove the obstacle from the the list

            //if don't have any other obstacles
            if(obstacles.Count == 0)
            {
                parentRenderer.sortingOrder = 100;
            }
            else //we have other obstacles and we need to change the sort order based on the obstacles
            {
                obstacles.Sort();
                parentRenderer.sortingOrder = obstacles[0].MySpriteRenderer.sortingOrder - 1;
            }
            
        }
        
    }
}
