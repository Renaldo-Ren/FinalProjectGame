using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void HealthChanged(float health);
public delegate void CharacterRemoved();

public class NPC : Character, IInteractable
{
    public event HealthChanged hpChanged;
    public event CharacterRemoved charRemoved;

    [SerializeField]
    private string name;

    public string myName { get => name; }

    public virtual void DeSelect()
    {
        hpChanged -= new HealthChanged(UIManage.myInstance.UpdateTargetFrame);
        charRemoved -= new CharacterRemoved(UIManage.myInstance.HideTargetFrame);
    }
    public virtual Character Select()
    {
        return this;
    }

    public void OnHealthChanged(float hp)
    {
        if(hpChanged != null)
        {
            hpChanged(hp);
        }
        
    }
    public void OnCharacterRemoved()
    {
        if(charRemoved != null)
        {
            charRemoved();
        }
        Destroy(gameObject);
    }

    public virtual void Interact()
    {
        
    }

    public virtual void StopInteract()
    {
        
    }
}
