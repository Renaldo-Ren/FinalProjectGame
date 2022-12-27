using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//interface define some functions that you implement in the interface need to have
public interface IState
{
    void Enter(Enemy parent);
    void Update();
    void Exit();

}
