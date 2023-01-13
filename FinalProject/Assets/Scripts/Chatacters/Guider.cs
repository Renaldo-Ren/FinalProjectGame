using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guider : NPC, IInteractable
{
    [SerializeField]
    private Dialog dialog;

    private static Guider instance;
    public static Guider MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Guider>();
            }
            return instance;
        }
    }

    public override void Interact()
    {
        base.Interact();
        Isinteracting = true;
        DialogWindow.MyInstance.SetDialog(dialog);
    }

    public override void StopInteract()
    {
        base.StopInteract();
        Isinteracting = false;
    }
}
