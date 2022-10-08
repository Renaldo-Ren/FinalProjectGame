using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    [SerializeField]
    private CanvasGroup HPgroup;

    public override Transform Select()
    {
        HPgroup.alpha = 1;
        return base.Select();
    }
    public override void DeSelect()
    {
        HPgroup.alpha = 0;
        base.DeSelect();
    }
}
