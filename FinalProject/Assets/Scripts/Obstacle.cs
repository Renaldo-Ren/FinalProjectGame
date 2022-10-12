using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IComparable<Obstacle>
{
    public SpriteRenderer MySpriteRenderer { get; set; }

    private Color defaultColor;
    private Color fadedColor;
    // Start is called before the first frame update
    void Start()
    {
        //MySpriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = MySpriteRenderer.color;
        fadedColor = defaultColor;
        fadedColor.a = 0.7f;
    }
    public void FadeOut()
    {
        MySpriteRenderer.color = fadedColor;
    }
    public void Fadein()
    {
        MySpriteRenderer.color = defaultColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int CompareTo(Obstacle other)
    {
        if(MySpriteRenderer.sortingOrder > other.MySpriteRenderer.sortingOrder)
        {
            return 1; //If this obstacles has a higher sort order
        }
        else if(MySpriteRenderer.sortingOrder < other.MySpriteRenderer.sortingOrder)
        {
            return -1; //If this obstacles has a lower sort order
        }
        return 0; //If both obstacles has an equal sort order
    }
}
