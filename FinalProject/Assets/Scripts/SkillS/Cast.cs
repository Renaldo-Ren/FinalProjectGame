using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Cast
{
    [SerializeField]
    private string name;

    [SerializeField]
    private int damage;

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float castTime;

    [SerializeField]
    private float castCD;

    [SerializeField]
    private bool checkCD;

    [SerializeField]
    private GameObject castPrefab;

    [SerializeField]
    private Color barColor;

    [SerializeField]
    private int manaCost;

    [SerializeField]
    private bool checkManaSufficient;

    public string myName { get => name; }
    public int myDamage { get => damage; }
    public Sprite myIcon { get => icon; }
    public float mySpeed { get => speed; }
    public float myCastTime { get => castTime; }
    public float myCastCD { get => castCD; }
    public bool myCheckCD { get => checkCD; set => checkCD = value; }
    public GameObject myCastPrefab { get => castPrefab; }
    public Color myBarColor { get => barColor; }
    public int myManaCost { get => manaCost; set => manaCost = value; }
    public bool myCheckManaSufficient { get => checkManaSufficient; set => checkManaSufficient = value; }
}

