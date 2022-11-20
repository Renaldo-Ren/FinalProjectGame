using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guider : NPC, IInteractable
{
    [SerializeField]
    private Dialog dialog;

    public override void Interact()
    {
        base.Interact();
        //Debug.Log("interactable");
        Isinteracting = true;
        DialogWindow.MyInstance.SetDialog(dialog);
    }

    public override void StopInteract()
    {
        base.StopInteract();
        Isinteracting = false;
        //Debug.Log("Stop interactable");
    }
}
