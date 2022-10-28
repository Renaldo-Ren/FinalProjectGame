using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guider : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("interactable");
    }

    public void StopInteract()
    {
        Debug.Log("Stop interactable");
    }
}
