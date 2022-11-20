using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    [SerializeField]
    private DialogNode[] nodes;

    public DialogNode[] Nodes { get => nodes; set => nodes = value; }
}
