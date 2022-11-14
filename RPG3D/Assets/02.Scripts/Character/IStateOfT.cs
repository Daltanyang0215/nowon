using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T> where T : Enum
{
    public T stateType { get; }
    public bool canExecute { get; }

    public void Execute();
    public void Stop();
    public T Tick();
    public IEnumerator<T> Workflow();
}
