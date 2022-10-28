using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Node
{
    //Use to find the path
    public int G { get; set; }
    public int H { get; set; }
    public int F { get; set; }
    public Node Parent { get; set; } //to find way back to parent is
    public Vector3Int Position { get; set; } //position in our grid

    public Node(Vector3Int position)
    {
        this.Position = position;
    }
}
