using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachineBase<T> : IStateMachine<T> where T : Enum
{
    public T currntType { get; protected set; }
    public IState<T> current { get; protected set; }
    public Dictionary<T, IState<T>> states { get; protected set; }
    public GameObject owner;
    public StateMachineBase(GameObject owner)
    {
        this.owner = owner;
    }
    public void Tick()
    {
        ChangeState(current.Tick());
    }

    public void ChangeState(T newStateType)
    {
        if (EqualityComparer<T>.Default.Equals(currntType, newStateType))
            return;

        if (states[newStateType].canExecute)
        {
            current.Stop();
            current = states[newStateType];
            current.Execute();
            currntType = newStateType;
        }
    }
    protected abstract void InitStates();

    public void ChangeState(object newStateType) => ChangeState((T)newStateType);
}
