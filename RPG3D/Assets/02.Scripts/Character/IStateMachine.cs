using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachine
{
    public void Tick();
    public void ChangeState(object newStateType);
}
